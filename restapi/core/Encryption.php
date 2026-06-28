<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

class Encryption {
    private $key;
    private $cipher;

    public function __construct($key = AES_SECRET_KEY, $cipher = AES_CIPHER) {
        $this->key    = $key;
        $this->cipher = $cipher;
    }

    public function encrypt($plaintext) {
        $iv         = random_bytes(16);
        $ciphertext = openssl_encrypt(
            $plaintext,
            $this->cipher,
            $this->key,
            OPENSSL_RAW_DATA,
            $iv
        );
        if ($ciphertext === false) {
            throw new Exception('Encryption failed.');
        }
        return base64_encode($iv . $ciphertext);
    }

    public function decrypt($stored) {
        $decoded    = base64_decode($stored);
        $iv         = substr($decoded, 0, 16);
        $ciphertext = substr($decoded, 16);
        $plaintext  = openssl_decrypt(
            $ciphertext,
            $this->cipher,
            $this->key,
            OPENSSL_RAW_DATA,
            $iv
        );
        if ($plaintext === false) {
            throw new Exception('Decryption failed.');
        }
        return $plaintext;
    }
}