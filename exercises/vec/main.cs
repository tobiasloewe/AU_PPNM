using static System.Console;
using static System.Math;
static class main{

public static void print(this double x, string s=""){Write(s);WriteLine(x);}

static int Main(){
	var rnd=new System.Random();
	var u=new vec(rnd.NextDouble(),rnd.NextDouble(),rnd.NextDouble());
	var v=new vec(rnd.NextDouble(),rnd.NextDouble(),rnd.NextDouble());
	u.print("u=");
	v.print("u=");
	WriteLine($"u={u}");
	WriteLine($"v={v}");
	WriteLine();
	vec t;

	t=new vec(-u.x,-u.y,-u.z);
	(-u).print("-u =");
	t.print   ("t  =");
	if(vec.approx(t,-u))WriteLine("test 'unary -' passed\n");

	t=new vec(u.x-v.x,u.y-v.y,u.z-v.z);
	(u-v).print("u-v =");
	t.print    ("t   =");
	if(vec.approx(t,u-v))WriteLine("test 'operator-' passed\n");

	t=new vec(u.x+v.x,u.y+v.y,u.z+v.z);
	(u+v).print("u+v =");
	t.print    ("t   =");
	if(vec.approx(t,u+v))WriteLine("test 'operator+' passed\n");

	double c=rnd.NextDouble();
	t=new vec(u.x*c,u.y*c,u.z*c);
	var tmp=u*c;
	tmp.print("u*c =");
	t.print  ("t   =");
	if(vec.approx(t,u*c))WriteLine("test 'operator*' passed\n");

	double d = u.x*v.x+u.y*v.y+u.z*v.z;
	(u%v).print("u%v=");
	(u.dot(v)).print("u.dot(v)=");
	d.print    ("d  =");
	if( vec.approx(d, u%v) )WriteLine("test 'operator%' passed");
    if( vec.approx(d, u.dot(v)))WriteLine("test '.dot()' passed\n");

	return 0;
	}
}//main