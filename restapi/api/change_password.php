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

if (empty($input['current_password']) || empty($input['new_password'])) {
    Response::error('Missing required fields.', 400);
}

$currentPassword = $input['current_password'];
$newPassword     = $input['new_password'];

if (strlen($newPassword) < 5 || strlen($newPassword) > 20) {
    Response::error('Password must be between 5 and 20 characters.', 400);
}

$encryption = new Encryption();
$repository = new UserRepository($db, $encryption);

try {
    $emailHash = hash('sha256', strtolower(trim($input['email'] ?? '')));
    $user      = $repository->findUserByEmailHash($emailHash);

    if (!$user || $user['id'] !== $userId) {
        Response::error('Invalid request.', 401);
    }

    // Verify current password against main password or temp password
    $verified = password_verify($currentPassword, $user['password_hash']);
    if (!$verified && !empty($user['temp_password_hash'])) {
        $verified = password_verify($currentPassword, $user['temp_password_hash']);
    }

    if (!$verified) {
        Response::error('Current password is incorrect.', 401);
    }

    $newPasswordHash = password_hash($newPassword, PASSWORD_BCRYPT);
    $repository->clearTempPassword($userId, $newPasswordHash);

    Response::success(['message' => 'Password changed successfully.']);
} catch (PDOException $e) {
    error_log('HTracker Change Password Error: ' . $e->getMessage());
    Response::serverError();
}