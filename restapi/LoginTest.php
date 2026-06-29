<?php
define('HTRACKER_ACCESS', true);
define('PHPUNIT_RUNNING', true);
define('HTRACKER_ROOT', __DIR__ . '/..');
define('AES_SECRET_KEY', 'da2c2d567fca5284518387e7fcff130f');
define('AES_CIPHER',     'AES-256-CBC');
define('API_VERSION',    '1.0.0');

require_once HTRACKER_ROOT . '/vendor/autoload.php';
require_once HTRACKER_ROOT . '/config/database.php';
require_once HTRACKER_ROOT . '/core/Encryption.php';
require_once HTRACKER_ROOT . '/core/Response.php';
require_once HTRACKER_ROOT . '/src/Repository/UserRepository.php';

use PHPUnit\Framework\TestCase;

class LoginTest extends TestCase {
    private $db;
    private $repository;
    private $testUserId;
    private $testEmailHash;

    protected function setUp(): void {
        $dotenv = Dotenv\Dotenv::createImmutable(HTRACKER_ROOT, '.env.testing');
        $dotenv->load();

        $database = new Database();
        $this->db = $database->getConnection();
        $this->db->beginTransaction();

        $encryption       = new Encryption();
        $this->repository = new UserRepository($this->db, $encryption);

        $this->testEmailHash = hash('sha256', 'brian@example.com');

        $this->testUserId = $this->repository->createUser(
            'Brian',
            'Smith',
            'brian@example.com',
            $this->testEmailHash,
            password_hash('Secret1!', PASSWORD_BCRYPT),
            bin2hex(random_bytes(32))
        );
    }

    protected function tearDown(): void {
        $this->db->rollBack();
    }

    public function testFindUserByEmailHashReturnsCorrectUser(): void {
        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertNotFalse($user);
        $this->assertEquals($this->testUserId, $user['id']);
    }

    public function testFindUserByEmailHashReturnsForcePasswordResetAsFalse(): void {
        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertEquals(0, $user['force_password_reset']);
    }

    public function testFindUserByEmailHashReturnsEmailVerifiedAsFalse(): void {
        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertEquals(0, $user['email_verified']);
    }

    public function testPasswordVerifiesCorrectly(): void {
        $user      = $this->repository->findUserByEmailHash($this->testEmailHash);
        $verified  = password_verify('Secret1!', $user['password_hash']);
        $this->assertTrue($verified);
    }

    public function testWrongPasswordDoesNotVerify(): void {
        $user     = $this->repository->findUserByEmailHash($this->testEmailHash);
        $verified = password_verify('WrongPassword1!', $user['password_hash']);
        $this->assertFalse($verified);
    }

    public function testTempPasswordVerifiesAfterForgotPassword(): void {
        $tempPassword = 'TempPass123';
        $tempHash     = password_hash($tempPassword, PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);

        $user     = $this->repository->findUserByEmailHash($this->testEmailHash);
        $verified = password_verify($tempPassword, $user['temp_password_hash']);
        $this->assertTrue($verified);
    }

    public function testMainPasswordStillVerifiesAfterTempPasswordSet(): void {
        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);

        $user     = $this->repository->findUserByEmailHash($this->testEmailHash);
        $verified = password_verify('Secret1!', $user['password_hash']);
        $this->assertTrue($verified);
    }

    public function testFindUserByEmailHashReturnsFalseForInactiveUser(): void {
        $stmt = $this->db->prepare('UPDATE ht_users SET is_active = 0 WHERE id = :id');
        $stmt->execute([':id' => $this->testUserId]);

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertFalse($user);
    }

    public function testFindUserByEmailHashReturnsFalseForDeletedUser(): void {
        $stmt = $this->db->prepare('UPDATE ht_users SET is_deleted = 1 WHERE id = :id');
        $stmt->execute([':id' => $this->testUserId]);

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertFalse($user);
    }
}