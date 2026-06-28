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

class UserRepositoryTest extends TestCase {
    private $db;
    private $repository;

    protected function setUp(): void {
        $dotenv = Dotenv\Dotenv::createImmutable(HTRACKER_ROOT, '.env.testing');
        $dotenv->load();

        $database = new Database();
        $this->db = $database->getConnection();
        $this->db->beginTransaction();

        $encryption       = new Encryption();
        $this->repository = new UserRepository($this->db, $encryption);
    }

    protected function tearDown(): void {
        $this->db->rollBack();
    }

    public function testCreateUserReturnsNewId(): void {
        $id = $this->repository->createUser(
            'Brian',
            'Smith',
            'brian@example.com',
            hash('sha256', 'brian@example.com'),
            password_hash('secret123', PASSWORD_BCRYPT),
            bin2hex(random_bytes(32))
        );

        $this->assertGreaterThan(0, $id);
    }

    public function testCreatedUserCanBeRetrievedById(): void {
        $email     = 'brian@example.com';
        $emailHash = hash('sha256', $email);

        $id = $this->repository->createUser(
            'Brian',
            'Smith',
            $email,
            $emailHash,
            password_hash('secret123', PASSWORD_BCRYPT),
            bin2hex(random_bytes(32))
        );

        $user = $this->repository->getUserById($id);

        $this->assertNotFalse($user);
        $this->assertEquals($id, $user['id']);
        $this->assertEquals($emailHash, $user['email_hash']);
        $this->assertEquals(1, $user['is_active']);
    }

    public function testEmailExistsReturnsTrueForDuplicate(): void {
        $emailHash = hash('sha256', 'brian@example.com');

        $this->repository->createUser(
            'Brian',
            'Smith',
            'brian@example.com',
            $emailHash,
            password_hash('secret123', PASSWORD_BCRYPT),
            bin2hex(random_bytes(32))
        );

        $this->assertTrue($this->repository->emailExists($emailHash));
    }

    public function testEmailExistsReturnsFalseForNewEmail(): void {
        $emailHash = hash('sha256', 'newuser@example.com');
        $this->assertFalse($this->repository->emailExists($emailHash));
    }
}