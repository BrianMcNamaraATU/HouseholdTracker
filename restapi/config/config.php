<?php

// Prevent direct access
if (!defined('HTRACKER_ACCESS')) {
    http_response_code(403);
    exit('Forbidden');
}

// API version
define('API_VERSION', '1.0.0');

// Timezone
define('LOCAL_TIMEZONE', 'Europe/Dublin');

// AES encryption key for personal data encryption
define('AES_SECRET_KEY', 'da2c2d567fca5284518387e7fcff130f');
define('AES_CIPHER',     'AES-256-CBC');

// API key header name
define('API_KEY_HEADER', 'X-System-Api-Key');

// Connection to central database
define('HTRACKER_APP_KEY', 'f22144d63501e2e6c8306fbe459ae234177bab511d092177b44e6e8fab1207ff');
define('HTRACKER_API_URL', 'https://www.cdcsoftware.ie/HTracker/api');