<?php
header('Content-Type: application/json');
define('HTRACKER_ACCESS', true);
define('HTRACKER_ROOT', __DIR__ . '/..');

require_once HTRACKER_ROOT . '/vendor/autoload.php';
require_once HTRACKER_ROOT . '/config/config.php';
require_once HTRACKER_ROOT . '/config/database.php';
require_once HTRACKER_ROOT . '/core/Response.php';
require_once HTRACKER_ROOT . '/core/Auth.php';
require_once HTRACKER_ROOT . '/core/Encryption.php';
require_once HTRACKER_ROOT . '/src/Repository/UserRepository.php';

$dotenv = Dotenv\Dotenv::createImmutable(HTRACKER_ROOT);
$dotenv->load();

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    Response::error('Method not allowed.', 405);
}

$database = new Database();
$db       = $database->getConnection();
if ($db === null) Response::serverError();

$auth   = new Auth($db);
$apiKey = isset($_GET['key']) ? trim($_GET['key']) : null;
$auth->validateSystemKey($apiKey);

$input = json_decode(file_get_contents('php://input'), true);

if (empty($input['email']) || empty($input['password'])) {
    Response::error('Missing required fields.', 400);
}

$email     = strtolower(trim($input['email']));
$password  = $input['password'];
$emailHash = hash('sha256', $email);

$encryption = new Encryption();
$repository = new UserRepository($db, $encryption);

try {
    $user = $repository->findUserByEmailHash($emailHash);

    if (!$user) {
        Response::error('Invalid email or password.', 401);
    }

    // Check temp password first if force_password_reset is set
    $authenticated = false;
    if ($user['force_password_reset'] && !empty($user['temp_password_hash'])) {
        $authenticated = password_verify($password, $user['temp_password_hash']);
    }

    // Fall back to main password
    if (!$authenticated) {
        $authenticated = password_verify($password, $user['password_hash']);
    }

    if (!$authenticated) {
        Response::error('Invalid email or password.', 401);
    }

    Response::success([
        'id'                   => $user['id'],
        'first_name'           => $encryption->decrypt($user['first_name']),
        'last_name'            => $encryption->decrypt($user['last_name']),
        'api_key'              => $user['api_key'],
        'force_password_reset' => (bool) $user['force_password_reset'],
        'email_verified'       => (bool) $user['email_verified'],
    ]);
} catch (PDOException $e) {
    error_log('HTracker Login Error: ' . $e->getMessage());
    Response::serverError();
}