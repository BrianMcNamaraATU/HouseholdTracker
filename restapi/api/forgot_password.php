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
require_once HTRACKER_ROOT . '/core/Mailer.php';
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

if (empty($input['email'])) {
    Response::error('Missing required fields.', 400);
}

$email     = strtolower(trim($input['email']));
$emailHash = hash('sha256', $email);

$encryption = new Encryption();
$repository = new UserRepository($db, $encryption);

try {
    $user = $repository->findUserByEmailHash($emailHash);

    // Always return success to avoid exposing which emails are registered
    if (!$user) {
        Response::success(['message' => 'If an account exists for this email, a temporary password has been sent.']);
    }

    // Generate a 12 character random temporary password
    $characters   = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    $tempPassword = '';
    for ($i = 0; $i < 12; $i++) {
        $tempPassword .= $characters[random_int(0, strlen($characters) - 1)];
    }

    $tempPasswordHash = password_hash($tempPassword, PASSWORD_BCRYPT);
    $repository->saveTempPassword($emailHash, $tempPasswordHash);

    $firstName = $encryption->decrypt($user['first_name']);

    $mailer = new Mailer();
    $mailer->send(
        $email,
        $firstName,
        'HouseholdTracker — Password Reset',
        "Hi {$firstName},\n\n" .
        "A password reset was requested for your HouseholdTracker account.\n\n" .
        "Your temporary password is: {$tempPassword}\n\n" .
        "Please log in with this temporary password and you will be prompted to set a new one.\n\n" .
        "If you did not request this, please contact support.\n\n" .
        "The HouseholdTracker Team"
    );

    Response::success(['message' => 'If an account exists for this email, a temporary password has been sent.']);
} catch (PDOException $e) {
    error_log('HTracker Forgot Password Error: ' . $e->getMessage());
    Response::serverError();
}