<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

// Database credentials
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "user_data";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Retrieve user details from POST request
$user_username = isset($_POST['username']) ? $_POST['username'] : '';
$user_password = isset($_POST['password']) ? $_POST['password'] : ''; // Encrypt the password
$age = isset($_POST['age']) ? $_POST['age'] : '';
$gender = isset($_POST['gender']) ? $_POST['gender'] : '';
$security_answer_1 = isset($_POST['security_answer1']) ? $_POST['security_answer1'] : '';
$security_answer_2 = isset($_POST['security_answer2']) ? $_POST['security_answer2'] : '';
$security_answer_3 = isset($_POST['security_answer3']) ? $_POST['security_answer3'] : '';

// Debugging output to see received POST data
error_log("Received POST data: " . json_encode($_POST));

// Check for missing fields
$missing_fields = [];
if (empty($user_username)) $missing_fields[] = 'username';
if (empty($user_password)) $missing_fields[] = 'password';
if (empty($age)) $missing_fields[] = 'age';
if (empty($gender)) $missing_fields[] = 'gender';
if (empty($security_answer_1)) $missing_fields[] = 'security_answer1';
if (empty($security_answer_2)) $missing_fields[] = 'security_answer2';
if (empty($security_answer_3)) $missing_fields[] = 'security_answer3';

if (!empty($missing_fields)) {
    echo json_encode(['success' => false, 'message' => 'Missing required fields: ' . implode(', ', $missing_fields)]);
    exit;
}

// Check for duplicate username
$sql_check_username = "SELECT username FROM users WHERE username = '$user_username'";
$result_check_username = $conn->query($sql_check_username);

if ($result_check_username->num_rows > 0) {
    echo json_encode(['success' => false, 'message' => 'Username already exists']);
    exit;
}

// SQL query to insert user into users table
$sql_user = "INSERT INTO users (username, password, age, gender, security_question_1, security_question_2, security_question_3, answer_1, answer_2, answer_3) 
             VALUES ('$user_username', '$user_password', '$age', '$gender', 'Favourite Food', 'Favourite Place', 'Nick Name', '$security_answer_1', '$security_answer_2', '$security_answer_3')";

if ($conn->query($sql_user) === TRUE) {
    // SQL query to insert user into player table with initial score 0
    $sql_player = "INSERT INTO player (username, score) VALUES ('$user_username', 0)";
    
    if ($conn->query($sql_player) === TRUE) {
        echo json_encode(["New record created successfully"]);
    } else {
        echo json_encode(["success" => false, "message" => "Error registering player: " . $conn->error]);
    }
} else {
    echo json_encode(["success" => false, "message" => "Error registering user: " . $conn->error]);
}

$conn->close();
?>
