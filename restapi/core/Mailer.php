<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;

require_once HTRACKER_ROOT . '/core/Encryption.php';
require_once HTRACKER_ROOT . '/vendor/phpmailer/phpmailer/src/PHPMailer.php';
require_once HTRACKER_ROOT . '/vendor/phpmailer/phpmailer/src/SMTP.php';
require_once HTRACKER_ROOT . '/vendor/phpmailer/phpmailer/src/Exception.php';

class Mailer {
    private $db;

    public function __construct($db) {
        $this->db = $db;
    }

    private function getSettings() {
        $stmt = $this->db->prepare(
            'SELECT smtp_host, smtp_port, smtp_username, smtp_password,
                    from_address, from_name, use_tls
             FROM tbl_email_settings
             WHERE is_active = 1
             LIMIT 1'
        );
        $stmt->execute();
        $row = $stmt->fetch();
        if (!$row) {
            throw new Exception('No active email settings found.');
        }
        $row['smtp_password'] = Encryption::decrypt($row['smtp_password']);
        return $row;
    }

    public function send($toAddress, $toName, $subject, $body) {
        $settings = $this->getSettings();

        $mail = new PHPMailer(true);

        $mail->isSMTP();
        $mail->Host       = $settings['smtp_host'];
        $mail->SMTPAuth   = true;
        $mail->Username   = $settings['smtp_username'];
        $mail->Password   = $settings['smtp_password'];
        $mail->SMTPSecure = $settings['use_tls'] ? PHPMailer::ENCRYPTION_STARTTLS : PHPMailer::ENCRYPTION_SMTPS;
        $mail->Port       = (int)$settings['smtp_port'];

        $mail->setFrom($settings['from_address'], $settings['from_name']);
        $mail->addAddress($toAddress, $toName);

        $mail->Subject = $subject;
        $mail->Body    = $body;

        $mail->send();
        return true;
    }
}