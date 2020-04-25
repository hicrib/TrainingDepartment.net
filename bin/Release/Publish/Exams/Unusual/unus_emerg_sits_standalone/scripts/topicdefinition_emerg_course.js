//   define the start pages for each frame
/*
var  topic0_topic_nav = "intro.htm";
var  topic0_main = "intro.htm";

var  topic1_topic_nav = "module1/mod1_nav.htm"
var  topic1_main = "module1/mod1less1main.htm";

var  topic2_topic_nav = "module2/mod2_nav.htm"
var  topic2_main = "module2/mod2less1main.htm";

var  topic3_main = "module3/mod3less1main.htm";
var  topic3_topic_nav = "module3/mod3_nav.htm"
*/

var numoftopics=16;  // number of topics in this module
// following two both set here. Default is FNNNNNNN  etc where one letter for each Topic.  F for first
var topicsofthismodule = "NNNNNNNNNNNNNNNN"; // sets Status of completion etc F = finished, S = Started, N = not started
var topicscompletedstatus = "FFFFFFFFFFFFFFFF"; // easy way of checking if all topics ahave been completed used in euro_scorm.js
                                      // set here manually
						  
var lessonscompleted = new Array("NNNNNNNN","NNNNNN","NNNNNNNN","NNNNNNNNNN","NNNNNN","NNNNNNN","NNNNNNN","NNNNNNNN","NNNNNNNN","NNNNNN","NNNNNN","NNNNNNNNN","NNNNNNNN","NNNNNNN","NNNNNNNNN","NNNNNNN");
var lessonscompletedstatus = new Array("FFFFFFFF","FFFFFF","FFFFFFFF","FFFFFFFFFF","FFFFFF","FFFFFFF","FFFFFFF","FFFFFFFF","FFFFFFFF","FFFFFF","FFFFFF","FFFFFFFFF","FFFFFFFF","FFFFFFF","FFFFFFFFF","FFFFFFF");

var color_N = "#E9DCE0";
var color_S = "#E1E1CC";
var color_F = "#CEE1D8";

var flag_N = "images/usertracking/dummy.gif";
var flag_S = "images/usertracking/started.gif";
var flag_F = "images/usertracking/finished.gif";