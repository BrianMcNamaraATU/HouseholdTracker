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

if (empty($input['first_name']) || empty($input['last_name']) ||
    empty($input['email'])      || empty($input['password'])) {
    Response::error('Missing required fields.', 400);
}

$firstName = trim($input['first_name']);
$lastName  = trim($input['last_name']);
$email     = strtolower(trim($input['email']));
$password  = $input['password'];
$emailHash = hash('sha256', $email);

if (strlen($password) < 1 || strlen($password) > 20) {
    Response::error('Password must be between 1 and 20 characters.', 400);
}

$encryption = new Encryption();
$repository = new UserRepository($db, $encryption);

try {
    if ($repository->emailExists($emailHash)) {
        Response::error('A user with this email already exists.', 409);
    }

    $passwordHash = password_hash($password, PASSWORD_BCRYPT);
    $apiKey       = bin2hex(random_bytes(32));

    $newId = $repository->createUser(
        $firstName,
        $lastName,
        $email,
        $emailHash,
        $passwordHash,
        $apiKey
    );

    Response::success([
        'id'      => $newId,
        'api_key' => $apiKey
    ]);
} catch (PDOException $e) {
    error_log('HTracker Add User Error: ' . $e->getMessage());
    Response::serverError();
}