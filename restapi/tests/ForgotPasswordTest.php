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

class ForgotPasswordTest extends TestCase {
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

    public function testSaveTempPasswordReturnsTrueForActiveUser(): void {
        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $result   = $this->repository->saveTempPassword($this->testEmailHash, $tempHash);
        $this->assertTrue($result);
    }

    public function testSaveTempPasswordSetsForcePasswordReset(): void {
        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertEquals(1, $user['force_password_reset']);
    }

    public function testSaveTempPasswordStoresHash(): void {
        $tempPassword = 'TempPass123';
        $tempHash     = password_hash($tempPassword, PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertTrue(password_verify($tempPassword, $user['temp_password_hash']));
    }

    public function testSaveTempPasswordReturnsFalseForNonExistentEmail(): void {
        $result = $this->repository->saveTempPassword(
            hash('sha256', 'nonexistent@example.com'),
            password_hash('TempPass123', PASSWORD_BCRYPT)
        );
        $this->assertFalse($result);
    }

    public function testClearTempPasswordReturnsTrueOnSuccess(): void {
        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);

        $result = $this->repository->clearTempPassword(
            $this->testUserId,
            password_hash('NewSecret1!', PASSWORD_BCRYPT)
        );
        $this->assertTrue($result);
    }

    public function testClearTempPasswordClearsForcePasswordReset(): void {
        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);
        $this->repository->clearTempPassword(
            $this->testUserId,
            password_hash('NewSecret1!', PASSWORD_BCRYPT)
        );

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertEquals(0, $user['force_password_reset']);
    }

    public function testClearTempPasswordNullifiesTempPasswordHash(): void {
        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);
        $this->repository->clearTempPassword(
            $this->testUserId,
            password_hash('NewSecret1!', PASSWORD_BCRYPT)
        );

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertNull($user['temp_password_hash']);
    }

    public function testClearTempPasswordUpdatesPasswordHash(): void {
        $newPassword     = 'NewSecret1!';
        $newPasswordHash = password_hash($newPassword, PASSWORD_BCRYPT);

        $tempHash = password_hash('TempPass123', PASSWORD_BCRYPT);
        $this->repository->saveTempPassword($this->testEmailHash, $tempHash);
        $this->repository->clearTempPassword($this->testUserId, $newPasswordHash);

        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertTrue(password_verify($newPassword, $user['password_hash']));
    }

    public function testFindUserByEmailHashReturnsNullForUnknownEmail(): void {
        $user = $this->repository->findUserByEmailHash(hash('sha256', 'unknown@example.com'));
        $this->assertFalse($user);
    }

    public function testFindUserByEmailHashReturnsUserForKnownEmail(): void {
        $user = $this->repository->findUserByEmailHash($this->testEmailHash);
        $this->assertNotFalse($user);
        $this->assertEquals($this->testUserId, $user['id']);
    }
}