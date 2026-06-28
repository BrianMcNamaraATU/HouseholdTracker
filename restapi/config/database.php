<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

class Database {
    private $charset = 'utf8mb4';
    private $conn    = null;

    public function getConnection() {
        if ($this->conn !== null) {
            return $this->conn;
        }
        try {
            $dsn = 'mysql:host=' . getenv('DB_HOST') .
                   ';port='      . getenv('DB_PORT') .
                   ';dbname='    . getenv('DB_NAME') .
                   ';charset='   . $this->charset;
            $this->conn = new PDO($dsn, getenv('DB_USER'), getenv('DB_PASS'));
            $this->conn->setAttribute(PDO::ATTR_ERRMODE,            PDO::ERRMODE_EXCEPTION);
            $this->conn->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);
            $this->conn->setAttribute(PDO::ATTR_EMULATE_PREPARES,   false);
        } catch (PDOException $e) {
            error_log('HTracker DB Connection Error: ' . $e->getMessage());
            return null;
        }
        return $this->conn;
    }
}