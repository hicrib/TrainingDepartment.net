
<!-- Inserted by euro_scorm.js:
var mm_adl_API = null;
var currenttopic;  // used on exit as bookmark
var topicstatus;  // takes data from LMS suspend stores until module exit 
				  // is then written back to LMS suspend status
var sco_start;   // milliseconds at start of sco
var sco_finish;  // milliseconds at end of sco
var studentName;
				  
var topicstatus = "NNNNNNNNNNNNNNNN";

// mm_getAPI, which calls findAPI as needed
function mm_getAPI()
{
}

// returns LMS API object (or null if not found)
function findAPI(win)
{
}

// call LMSInitialize()
function mm_adlOnload()
{
}

// call LMSFinish()
function mm_adlOnunload()
{
}

function euro_get_hoursminutes(st,fi)
{
}

function euro_get_error()
{
}

function euro_get_name()
{
}

function euro_set_score(x)
{
}

function euro_get_entrystatus()
{
}

function euro_set_last(x)
{
}

function euro_get_last(x)
{
}

// checks LMS suspend_data.  This is used to store which topics have been completed
// Uses F = Finished, S = Started , N = Not started
// These are refrenced as a string with item 1 = Topic 1 etc
//  typical would be FSNNN  etc
function euro_get_storeddata()
{
}

function euro_set_storeddata(x)
{
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
	top.close();
}


// this version of exit routine to be attached to onUnLoad behaviour in body of main frame set, 
// to sent data back if the close the window. Only difference is that there is no close window command.
function euro_exitLMS2()
{
}

// get the API
//mm_getAPI();


// -->