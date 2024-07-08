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

// SQL query to fetch leaderboard data
$sql = "SELECT username, score FROM player ORDER BY score DESC";
$result = $conn->query($sql);

// Check if there are results
if ($result->num_rows > 0) {
    // Output data as JSON
    $rows = array();
    while ($row = $result->fetch_assoc()) {
        $rows[] = $row;
    }
    echo json_encode(['players' => $rows]);
} else {
    echo json_encode(['players' => []]);
}

$conn->close();
?>
