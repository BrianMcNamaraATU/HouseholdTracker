<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

class Response {
    public static function send($success, $data = null, $error = null, $httpCode = 200) {
        if (!defined('PHPUNIT_RUNNING')) {
            http_response_code($httpCode);
            header('Content-Type: application/json; charset=utf-8');
        }
        $response = [
            'success'     => $success,
            'api_version' => API_VERSION,
            'data'        => $data,
            'error'       => $error
        ];
        echo json_encode($response, JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES);
        if (!defined('PHPUNIT_RUNNING')) {
            exit();
        }
    }

    public static function success($data = null, $httpCode = 200) {
        self::send(true, $data, null, $httpCode);
    }

    public static function error($message, $httpCode = 400) {
        self::send(false, null, $message, $httpCode);
    }

    public static function unauthorized() {
        self::error('Invalid or missing API key.', 401);
    }

    public static function serverError() {
        self::error('A server error occurred. Please try again later.', 500);
    }
}