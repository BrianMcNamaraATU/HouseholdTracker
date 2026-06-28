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

$apiKey = isset($_GET['key'])     ? trim($_GET['key'])     : null;
$userId = isset($_GET['user_id']) ? (int)$_GET['user_id'] : 0;

$auth = new Auth($db);
$auth->validateUserSession($apiKey, $userId);

$input = json_decode(file_get_contents('php://input'), true);

if (empty($input['first_name']) || empty($input['last_name']) || empty($input['email'])) {
    Response::error('Missing required fields.', 400);
}

$firstName = trim($input['first_name']);
$lastName  = trim($input['last_name']);
$email     = strtolower(trim($input['email']));
$emailHash = hash('sha256', $email);

$encryption = new Encryption();
$repository = new UserRepository($db, $encryption);

try {
    if ($repository->emailExists($emailHash, $userId)) {
        Response::error('Email address is already in use.', 409);
    }

    $updated = $repository->updateProfile($userId, $firstName, $lastName, $email, $emailHash);

    if (!$updated) {
        Response::error('Profile could not be updated.', 400);
    }

    Response::success(['message' => 'Profile updated successfully.']);

} catch (PDOException $e) {
    error_log('HTracker Update Profile Error: ' . $e->getMessage());
    Response::serverError();
}