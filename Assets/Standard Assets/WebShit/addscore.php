<?php 
        $db = mysql_connect('instance40319.db.xeround.com:8092', 'ccgames', 'volunteer15') or die('Could not connect: ' . mysql_error()); 
        mysql_select_db('squirrel_scores') or die('Could not select database');
 
        // Strings must be escaped to prevent SQL injection attack. 
        $name = mysql_real_escape_string($_GET['name'], $db); 
        $score = mysql_real_escape_string($_GET['score'], $db); 
        $hash = $_GET['hash']; 
 
        $secretKey="POOhm0ox5^t!$c0vo505c@#i5hk+0f0bsrs!@7pdojf0ymf#2)oPOO"; 

        $real_hash = md5($name . $score . $secretKey); 
        if($real_hash == $hash) { 
            // Send variables for the MySQL database class. 
            $query = "insert into scores values (NULL, '$name', '$score');"; 
            $result = mysql_query($query) or die('Query failed: ' . mysql_error()); 
        } 
?>