using FanShapeCCD;
using FK_CLI;
using System;
using System.Collections.Generic;

//ウィンドウ変数
fk_AppWindow win = new fk_AppWindow();

var models = new List<fk_Model>();
var sphere = new fk_Sphere(8, 1.0);

for(int i = 0; i < 6; i++)
{
    var model = new fk_Model();
    model.Shape = sphere;
    models.Add(model);
}

fk_Vector A, B, C, D;
A = new fk_Vector(-10.0, -10.0, 0.0);
B = new fk_Vector(10.0, 10.0, 0.0);
C = new fk_Vector(-10.0, 10.0, 0.0);
D = new fk_Vector(10.0, -10.0, 0.0);

var AB = new fk_Line();
AB.SetVertex(0, A);
AB.SetVertex(1, B);
var CD = new fk_Line();
CD.SetVertex(0, C);
CD.SetVertex(1, D);

var lAB = new fk_Model();
lAB.Shape = AB;
lAB.Material = fk_Material.AshGray;
var lCD = new fk_Model();
lCD.Shape = CD;
lCD.Material = fk_Material.AshGray;

models[0].GlMoveTo(A);
models[1].GlMoveTo(B);
models[2].GlMoveTo(C);
models[3].GlMoveTo(D);

for(int i=0;i<4;i++)
{
    win.Entry(models[i]);
}

WindowSetup();

win.Entry(lAB);
win.Entry(lCD);

while (win.Update() == true)
{
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