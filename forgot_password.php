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

// Retrieve POST data
$username = $_POST['username'];
$answer1 = $_POST['answer_1'];
$answer2 = $_POST['answer_2'];
$answer3 = $_POST['answer_3'];

// SQL query to check if answers are correct
$sql = "SELECT * FROM users WHERE username = '$username' AND answer_1 = '$answer1' AND answer_2 = '$answer2' AND answer_3 = '$answer3'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // Answers are correct
    echo json_encode(array("status" => "success"));
} else {
    // Answers are incorrect
    echo json_encode(array("status" => "error", "message" => "Incorrect answers provided."));
}

$conn->close();
?>
