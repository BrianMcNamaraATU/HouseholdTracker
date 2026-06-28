<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

class UserRepository {
    private $db;
    private $encryption;

    public function __construct($db, $encryption) {
        $this->db         = $db;
        $this->encryption = $encryption;
    }

    public function emailExists($emailHash, $excludeUserId = null) {
        $sql = 'SELECT id FROM ht_users WHERE email_hash = :email_hash';
        if ($excludeUserId !== null) {
            $sql .= ' AND id != :exclude_id';
        }
        $sql .= ' LIMIT 1';

        $stmt = $this->db->prepare($sql);
        $params = [':email_hash' => $emailHash];
        if ($excludeUserId !== null) {
            $params[':exclude_id'] = $excludeUserId;
        }
        $stmt->execute($params);
        return $stmt->fetch() !== false;
    }

    public function createUser($firstName, $lastName, $email, $emailHash, $passwordHash, $apiKey) {
        $stmt = $this->db->prepare(
            'INSERT INTO ht_users
                (first_name, last_name, email, email_hash, password_hash, api_key,
                 is_active, created_at_utc, last_modified_local, last_modified_utc)
             VALUES
                (:first_name, :last_name, :email, :email_hash, :password_hash, :api_key,
                 1, UTC_TIMESTAMP(), NOW(), UTC_TIMESTAMP())'
        );
        $stmt->execute([
            ':first_name'    => $this->encryption->encrypt($firstName),
            ':last_name'     => $this->encryption->encrypt($lastName),
            ':email'         => $this->encryption->encrypt($email),
            ':email_hash'    => $emailHash,
            ':password_hash' => $passwordHash,
            ':api_key'       => $apiKey,
        ]);
        return (int) $this->db->lastInsertId();
    }

    public function getUserById($id) {
        $stmt = $this->db->prepare(
            'SELECT id, email_hash, is_active FROM ht_users
             WHERE id = :id LIMIT 1'
        );
        $stmt->execute([':id' => $id]);
        return $stmt->fetch();
    }

    public function findActiveUserById($id) {
        $stmt = $this->db->prepare(
            'SELECT id FROM ht_users
             WHERE id = :id
             AND is_active = 1
             AND is_deleted = 0
             LIMIT 1'
        );
        $stmt->execute([':id' => $id]);
        return $stmt->fetch();
    }

    public function updateProfile($userId, $firstName, $lastName, $email, $emailHash) {
        $stmt = $this->db->prepare(
            'UPDATE ht_users SET
                first_name          = :first_name,
                last_name           = :last_name,
                email               = :email,
                email_hash          = :email_hash,
                last_modified_local = NOW(),
                last_modified_utc   = UTC_TIMESTAMP()
             WHERE id = :id
             AND is_active = 1
             AND is_deleted = 0'
        );
        $stmt->execute([
            ':first_name' => $this->encryption->encrypt($firstName),
            ':last_name'  => $this->encryption->encrypt($lastName),
            ':email'      => $this->encryption->encrypt($email),
            ':email_hash' => $emailHash,
            ':id'         => $userId,
        ]);
        return $stmt->rowCount() > 0;
    }
}