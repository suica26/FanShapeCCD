using FanShapeCCD;
using FK_CLI;
using System;
using System.Collections.Generic;

//ウィンドウ変数
fk_AppWindow win = new fk_AppWindow();

var models = new List<fk_Model>();
var sphere = new fk_Sphere(8, 1.0);
var cSphere = new fk_Sphere(8, 1.1);

for (int i = 0; i < 6; i++)
{
    var model = new fk_Model();
    model.Shape = sphere;
    model.Material = fk_Material.Yellow;
    models.Add(model);
}

models[4].Shape = cSphere;
models[5].Shape = cSphere;

fk_Vector A, B, C, D, P, Q;
A = new fk_Vector(-10.0, -10.0, 10.0);
B = new fk_Vector(10.0, 10.0, 0.0);
C = new fk_Vector(-10.0, 10.0, 0.0);
D = new fk_Vector(10.0, -10.0, 0.0);
P = new fk_Vector(10.0, 0.0, 0.0);
Q = new fk_Vector(-10.0, 0.0, 0.0);

double s, t;
s = t = 0.0;

var AB = new fk_Line();
AB.PushLine(A, B);
var CD = new fk_Line();
CD.PushLine(C, D);

var lAB = new fk_Model();
lAB.Shape = AB;
lAB.LineColor = new fk_Color(1.0, 0.0, 0.0);
lAB.LineWidth = 1.0;
var lCD = new fk_Model();
lCD.Shape = CD;
lCD.LineColor = new fk_Color(0.0, 0.0, 1.0);
lCD.LineWidth = 1.0;

models[0].GlMoveTo(A);
models[1].GlMoveTo(B);
models[2].GlMoveTo(C);
models[3].GlMoveTo(D);
models[4].GlMoveTo(P);
models[5].GlMoveTo(Q);

models[4].Material = fk_Material.Red;
models[5].Material = fk_Material.Blue;

for (int i=0;i<6;i++)
{
    win.Entry(models[i]);
}

WindowSetup();

win.Entry(lAB);
win.Entry(lCD);

while (win.Update() == true)
{
    if (win.GetSpecialKeyStatus(fk_Key.UP)) A.y += 0.1;
    if (win.GetSpecialKeyStatus(fk_Key.DOWN)) A.y -= 0.1;
    if (win.GetSpecialKeyStatus(fk_Key.LEFT)) A.x -= 0.1;
    if (win.GetSpecialKeyStatus(fk_Key.RIGHT)) A.x += 0.1;
    models[0].GlMoveTo(A);
    AB.SetVertex(0, A);
    AB.SetVertex(1, B);
    CD.SetVertex(0, C);
    CD.SetVertex(1, D);

    DistCalc.CalcSegmentSegmentDist(A, B, C, D, ref P, ref Q, ref s, ref t);
    models[4].GlMoveTo(P);
    models[5].GlMoveTo(Q);
}

/////////////////////////////////////////////////////////////////////////////////

//ウィンドウ設定
void WindowSetup()
{
    win.CameraPos = new fk_Vector(0.0, 0.0, 100.0);
    win.CameraFocus = new fk_Vector(0.0, 0.0, 0.0);
    win.Size = new fk_Dimension(1000, 800);
    win.ShowGuide(fk_Guide.GRID_XY);
    win.BGColor = new fk_Color(0.6, 0.7, 0.8);
    win.TrackBallMode = true;
    win.Open();
}