<?php

use PHPUnit\Framework\TestCase;

class DatabaseConnectionTest extends TestCase
{
    private $pdo;

    protected function setUp(): void
    {
        $host = getenv('DB_HOST');
        $port = getenv('DB_PORT');
        $name = getenv('DB_NAME');
        $user = getenv('DB_USER');
        $pass = getenv('DB_PASS');

        $this->pdo = new PDO(
            "mysql:host=$host;port=$port;dbname=$name;charset=utf8",
            $user,
            $pass,
            [PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION]
        );
    }

    public function testConnectionIsSuccessful(): void
    {
        $this->assertInstanceOf(PDO::class, $this->pdo);
    }

    public function testCanQueryDatabase(): void
    {
        $stmt = $this->pdo->query("SELECT 1");
        $result = $stmt->fetchColumn();
        $this->assertEquals(1, $result);
    }
}