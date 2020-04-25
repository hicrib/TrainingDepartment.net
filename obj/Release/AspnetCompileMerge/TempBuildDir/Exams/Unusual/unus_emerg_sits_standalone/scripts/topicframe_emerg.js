<!--//  added for euro_scorm functionality
function topicpages(x)   // pages to got to in the various frames
{
	parent.currenttopic=x;
	if (x==0)  // objectives
	{
		euro_goToURL('parent.frames[\'topic_nav\']',parent.topic0_topic_nav);		
		euro_goToURL('parent.frames[\'main\']',parent.topic0_main);
	}
	else if (x==1) 
	{
		euro_goToURL('parent.frames[\'topic_nav\']',parent.topic1_topic_nav);		
		euro_goToURL('parent.frames[\'main\']',parent.topic1_main);
	}
	else if (x==2)
	{
		euro_goToURL('parent.frames[\'topic_nav\']',parent.topic2_topic_nav);		
		euro_goToURL('parent.frames[\'main\']',parent.topic2_main);
	}
	else if (x==3)
	{
		euro_goToURL('parent.frames[\'topic_nav\']',parent.topic3_topic_nav);		
		euro_goToURL('parent.frames[\'main\']',parent.topic3_main);
	}
}

function starttopics()
{
	var sta = parent.euro_get_entrystatus();
	if (sta=="resume")
	{
		startpoint = parent.euro_get_last();
		if (startpoint=="")
		{
			startpoint=0;
		}
		settopics();
		topicpages(startpoint);
	}
	else
	{
		settopics();
	}
}

function settopics()
{
	count = 0;
	while (count <=parent.numoftopics)
	{
//		cc = "t" + count;
		flag = "flag" + count	
		cp=parent.topicstatus.charAt(count);
		if (cp=="N")
		{
//		zzz = "document.all." + cc + ".bgColor = parent.color_N"; // //'#FADBDA'";
		zzzz = "document.all." + flag + ".src = parent.flag_N";
//		eval(zzz);
		eval(zzzz);
		}
		else if (cp=="S")
		{
//		zzz = "document.all." + cc + ".bgColor = parent.color_S"; //'#FFFFCC'";
		zzzz = "document.all." + flag + ".src = parent.flag_S";
//		eval(zzz);
		eval(zzzz);
		}
		else if (cp=="F")
		{
//		zzz = "document.all." + cc + ".bgColor = parent.color_F"; //'#D5FFD5'";
		zzzz = "document.all." + flag + ".src = parent.flag_F";
//		eval(zzz);
		eval(zzzz);
		}
		count++;
	}
}

//'checkem' function removed from here as not required for emergency

function euro_goToURL() { 
  var i, args=euro_goToURL.arguments; document.euro_returnValue = false;
  for (i=0; i<(args.length-1); i+=2) eval(args[i]+".location='"+args[i+1]+"'");
}

// -->
