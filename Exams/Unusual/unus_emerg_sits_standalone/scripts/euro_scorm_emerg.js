
<!-- Inserted by euro_scorm.js:
var mm_adl_API = null;
var currenttopic;  // used on exit as bookmark
var topicstatus;  // takes data from LMS suspend stores until module exit 
				  // is then written back to LMS suspend status
var sco_start;   // milliseconds at start of sco
var sco_finish;  // milliseconds at end of sco
var studentName;
				  


// mm_getAPI, which calls findAPI as needed
function mm_getAPI()
{
  var myAPI = null;
  var tries = 0, triesMax = 500;
  while (tries < triesMax && myAPI == null)
  {
    window.status = 'Looking for API object ' + tries + '/' + triesMax;
    myAPI = findAPI(window);
    if (myAPI == null && typeof(window.parent) != 'undefined') myAPI = findAPI(window.parent)
    if (myAPI == null && typeof(window.top) != 'undefined') myAPI = findAPI(window.top);
    if (myAPI == null && typeof(window.opener) != 'undefined') if (window.opener != null && !window.opener.closed) myAPI = findAPI(window.opener);
    tries++;
  }
  if (myAPI == null)
  {
    window.status = 'API not found';
    //alert('JavaScript Warning: API object not found in window or opener. (' + tries + ')');
  }
  else
  {
    mm_adl_API = myAPI;
    window.status = 'API found';
	mm_adlOnload();
  }
}

// returns LMS API object (or null if not found)
function findAPI(win)
{
  // look in this window
  if (typeof(win) != 'undefined' ? typeof(win.API) != 'undefined' : false)
  {
    if (win.API != null )  return win.API;
  }
  // look in this window's frameset kin (except opener)
  if (win.frames.length > 0)  for (var i = 0 ; i < win.frames.length ; i++);
  {
    if (typeof(win.frames[i]) != 'undefined' ? typeof(win.frames[i].API) != 'undefined' : false)
    {
	     if (win.frames[i].API != null)  return win.frames[i].API;
    }
  }
  return null;
}

// call LMSInitialize()
function mm_adlOnload()
{
	////alert("got this far");
  if (mm_adl_API != null)
  {
    mm_adl_API.LMSInitialize("");
	euro_get_storeddata();
	euro_get_name();
	// get start time in milliseconds
	now = new Date();
	sco_start=now.getTime();
    // set status;
	////alert("initialise stage");
  }
}

// call LMSFinish()
function mm_adlOnunload()
{
  if (mm_adl_API != null)
  {
    now= new Date();
	sco_finish =now.getTime();
	elapsedtime = euro_get_hoursminutes(sco_finish,sco_start);
	euro_set_last(currenttopic);
	euro_set_storeddata(topicstatus);
	
	mm_adl_API.LMSSetValue("cmi.core.session_time", elapsedtime);
    mm_adl_API.LMSSetValue("cmi.core.exit", "suspend");
    mm_adl_API.LMSCommit("");
    mm_adl_API.LMSFinish("");
  }
}

function euro_get_hoursminutes(st,fi)
{
	ems =st-fi;
	esecs=Math.floor(ems/1000);
	secs=esecs%60;
	if (secs<=9)
	{
		secs = "0" + secs;
	}
	allmins=Math.floor(esecs/60);
	mins=allmins%60;
	if (mins<=9)
	{
		mins = "0" + mins;
	}
	hrs=Math.floor(allmins/60);
	if (hrs<=9)
	{
		hrs = "0" + hrs;
	}
	str=hrs +":" + mins +":" +secs;
	return str;
}

function euro_get_error()
{
	if (mm_adl_API != null)
  	{
   	 // set status
	 	errorstring="";
   		errorcode = mm_adl_API.LMSGetLastError();
		errorstring = mm_adl_API.LMSGetErrorString(errorcode);
	//alert (sname);
   	 	window.status = errorstring;
  	}


}

function euro_get_name()
{
	if (mm_adl_API != null)
  	{
   	 // set status
   	studentName = mm_adl_API.LMSGetValue("cmi.core.student_name");
	euro_get_error();
   	 //alert (sname);
  	}
}

function euro_set_score(x)
{
	if (mm_adl_API != null)
  	{
	//set status
  	var sname = mm_adl_API.LMSSetValue("cmi.core.score.raw",x);
	euro_get_error();
   	}
}
function euro_get_entrystatus()
{
   	entrys = mm_adl_API.LMSGetValue("cmi.core.entry");
	return entrys;
}

function euro_set_last(x)
{
	if (mm_adl_API != null)
  	{
	  	var sname = mm_adl_API.LMSSetValue("cmi.core.lesson_location",x);
		euro_get_error();
   	}
}
function euro_get_last(x)
{
	if (mm_adl_API != null)
  	{
  		var sname = mm_adl_API.LMSgetValue("cmi.core.lesson_location");
		euro_get_error();
		return sname;
   	}
}

// checks LMS suspend_data.  This is used to store which topics have been completed
// Uses F = Finished, S = Started , N = Not started
// These are refrenced as a string with item 1 = Topic 1 etc
//  typical would be FSNNN  etc
function euro_get_storeddata()
{
	if (mm_adl_API != null)
	{
		var sname = mm_adl_API.LMSgetValue("cmi.suspend_data");
		euro_get_error();
		if (sname=="")
		{
			// set the suspend_data to indicate Not Started for all modules.. i.e. rosy background
			// firts is ONE page objectives and is set as complete by default
			topicstatus = topicsofthismodule;
			euro_set_storeddata(topicstatus);
		}
		else
		{
			topicstatus = sname;
		}
	    return topicstatus;
 	}
}
function euro_set_storeddata(x)
{
	if (mm_adl_API != null)
 	{
 	 	var sname = mm_adl_API.LMSsetValue("cmi.suspend_data",x);
   	}
}
function euro_replacestringitem(st,i,l)
{
	sl=st.length;
	if (i==0)
	{
		bita = st.slice(1,sl);
		newbita = l + bita;
		return newbita;
	}
	else if (i==sl)
	{
		bita = st.slice(0,sl-1);
		newbita = bita + l;
		return newbita;
	}
	else 
	{
		bita1 = st.slice(0,i);
		bita2 = st.slice(i+1);
		newbita = bita1 + l + bita2;
		return newbita;
	}
}

function euro_set_lessoncomplete(x,y) //y is the lesson number
{
	l=lessonscompleted[x];
	s="F";
	s2="S"
	l = euro_replacestringitem(l,y,s);
	lessonscompleted[x] = l;

	t=topicstatus;
	if(lessonscompleted[x]==lessonscompletedstatus[x])
		{
		t=euro_replacestringitem(t,x,s);
		topicstatus=t;
		}
	else
		{
		t=euro_replacestringitem(t,x,s2);
		topicstatus=t;
		}
	// set stored data called here as well as at exitLMS in case user refreshes or closes
	euro_set_storeddata(topicstatus);
	navbar.settopics();

//	window.alert(x + lessonscompleted[x]);
}


function euro_set_topiccomplete(x) // x is the topic/module number
{
	t=topicstatus;
	s="F"
	t = euro_replacestringitem(t,x,s);
	topicstatus=t;
	topic.settopics();
}


function euro_exitLMS()
{
	if (mm_adl_API != null)
	{
		if (topicstatus == topicscompletedstatus) // all topics finished
  		{
	  		var sname = mm_adl_API.LMSSetValue("cmi.core.lesson_status","completed");
		}
		else
		{
			var sname = mm_adl_API.LMSSetValue("cmi.core.lesson_status","incomplete");
		}
   	}
	mm_adlOnunload();
	top.close();
}
// this version of exit routine to be attached to onUnLoad behaviour in body of main frame set, 
// to sent data back if the close the window. Only difference is that there is no close window command.

function euro_exitLMS2()
{
	if (mm_adl_API != null)
	{
		if (topicstatus == topicscompletedstatus) // all topics finished
  		{
	  		var sname = mm_adl_API.LMSSetValue("cmi.core.lesson_status","completed");
		}
		else
		{
			var sname = mm_adl_API.LMSSetValue("cmi.core.lesson_status","incomplete");
		}
   	}
	mm_adlOnunload();
}

// get the API
mm_getAPI();


// -->