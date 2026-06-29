<?php
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;

require_once HTRACKER_ROOT . '/vendor/phpmailer/phpmailer/src/PHPMailer.php';
require_once HTRACKER_ROOT . '/vendor/phpmailer/phpmailer/src/SMTP.php';
require_once HTRACKER_ROOT . '/vendor/phpmailer/phpmailer/src/Exception.php';

class Mailer {
    private $smtpHost;
    private $smtpPort;
    private $smtpUsername;
    private $smtpPassword;
    private $fromAddress;
    private $fromName;

    public function __construct() {
        $this->smtpHost     = getenv('SMTP_HOST');
        $this->smtpPort     = (int) getenv('SMTP_PORT');
        $this->smtpUsername = getenv('SMTP_USERNAME');
        $this->smtpPassword = getenv('SMTP_PASSWORD');
        $this->fromAddress  = getenv('SMTP_FROM_ADDRESS');
        $this->fromName     = getenv('SMTP_FROM_NAME');
    }

    public function send($toAddress, $toName, $subject, $body) {
        $mail = new PHPMailer(true);
        $mail->isSMTP();
        $mail->Host       = $this->smtpHost;
        $mail->SMTPAuth   = true;
        $mail->Username   = $this->smtpUsername;
        $mail->Password   = $this->smtpPassword;
        $mail->SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS;
        $mail->Port       = $this->smtpPort;
        $mail->setFrom($this->fromAddress, $this->fromName);
        $mail->addAddress($toAddress, $toName);
        $mail->Subject = $subject;
        $mail->Body    = $body;
        $mail->send();
        return true;
    }
}