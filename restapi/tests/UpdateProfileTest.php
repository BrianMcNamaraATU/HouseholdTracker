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

class UpdateProfileTest extends TestCase {
    private $db;
    private $repository;
    private $testUserId;

    protected function setUp(): void {
        $dotenv = Dotenv\Dotenv::createImmutable(HTRACKER_ROOT, '.env.testing');
        $dotenv->load();

        $database = new Database();
        $this->db = $database->getConnection();
        $this->db->beginTransaction();

        $encryption       = new Encryption();
        $this->repository = new UserRepository($this->db, $encryption);

        // Insert a base user to update in each test
        $this->testUserId = $this->repository->createUser(
            'Brian',
            'Smith',
            'brian@example.com',
            hash('sha256', 'brian@example.com'),
            password_hash('secret123', PASSWORD_BCRYPT),
            bin2hex(random_bytes(32))
        );
    }

    protected function tearDown(): void {
        $this->db->rollBack();
    }

    public function testUpdateProfileReturnsTrueOnSuccess(): void {
        $result = $this->repository->updateProfile(
            $this->testUserId,
            'Brian',
            'Updated',
            'brian.updated@example.com',
            hash('sha256', 'brian.updated@example.com')
        );

        $this->assertTrue($result);
    }

    public function testUpdateProfileChangesEmailHash(): void {
        $newEmail     = 'brian.new@example.com';
        $newEmailHash = hash('sha256', $newEmail);

        $this->repository->updateProfile(
            $this->testUserId,
            'Brian',
            'Smith',
            $newEmail,
            $newEmailHash
        );

        $user = $this->repository->getUserById($this->testUserId);
        $this->assertEquals($newEmailHash, $user['email_hash']);
    }

    public function testUpdateProfileReturnsFalseForInactiveUser(): void {
        // Deactivate the user directly
        $stmt = $this->db->prepare(
            'UPDATE ht_users SET is_active = 0 WHERE id = :id'
        );
        $stmt->execute([':id' => $this->testUserId]);

        $result = $this->repository->updateProfile(
            $this->testUserId,
            'Brian',
            'Smith',
            'brian@example.com',
            hash('sha256', 'brian@example.com')
        );

        $this->assertFalse($result);
    }

    public function testUpdateProfileReturnsFalseForDeletedUser(): void {
        // Mark the user as deleted
        $stmt = $this->db->prepare(
            'UPDATE ht_users SET is_deleted = 1 WHERE id = :id'
        );
        $stmt->execute([':id' => $this->testUserId]);

        $result = $this->repository->updateProfile(
            $this->testUserId,
            'Brian',
            'Smith',
            'brian@example.com',
            hash('sha256', 'brian@example.com')
        );

        $this->assertFalse($result);
    }

    public function testEmailExistsReturnsTrueForAnotherUsersEmail(): void {
        // Create a second user
        $this->repository->createUser(
            'Jane',
            'Doe',
            'jane@example.com',
            hash('sha256', 'jane@example.com'),
            password_hash('secret456', PASSWORD_BCRYPT),
            bin2hex(random_bytes(32))
        );

        // Try to use Jane's email for Brian's update
        $result = $this->repository->emailExists(
            hash('sha256', 'jane@example.com'),
            $this->testUserId
        );

        $this->assertTrue($result);
    }

    public function testEmailExistsReturnsFalseForOwnEmail(): void {
        // User keeping their own email should not be blocked
        $result = $this->repository->emailExists(
            hash('sha256', 'brian@example.com'),
            $this->testUserId
        );

        $this->assertFalse($result);
    }
}