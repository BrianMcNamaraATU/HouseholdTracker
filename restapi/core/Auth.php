<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

class Auth {
    private $db;

    public function __construct($db) {
        $this->db = $db;
    }

    public function validateSystemKey($apiKey) {
        if (empty($apiKey)) {
            Response::unauthorized();
            return null;
        }
        try {
            $stmt = $this->db->prepare(
                'SELECT id FROM tbl_system_clients
                 WHERE api_key = :api_key
                 AND is_active = 1
                 LIMIT 1'
            );
            $stmt->execute([':api_key' => $apiKey]);
            $client = $stmt->fetch();
            if (!$client) {
                Response::unauthorized();
                return null;
            }
            return $client['id'];
        } catch (PDOException $e) {
            error_log('HTracker Auth Error: ' . $e->getMessage());
            Response::serverError();
            return null;
        }
    }

    public function validateUserSession($apiKey, $userId) {
        if (empty($apiKey) || $userId === 0) {
            Response::unauthorized();
            return null;
        }
        try {
            $stmt = $this->db->prepare(
                'SELECT id FROM ht_users
                 WHERE id = :id
                 AND api_key = :api_key
                 AND is_active = 1
                 AND is_deleted = 0
                 LIMIT 1'
            );
            $stmt->execute([':id' => $userId, ':api_key' => $apiKey]);
            $user = $stmt->fetch();
            if (!$user) {
                Response::unauthorized();
                return null;
            }
            return $user['id'];
        } catch (PDOException $e) {
            error_log('HTracker Auth User Error: ' . $e->getMessage());
            Response::serverError();
            return null;
        }
    }
}