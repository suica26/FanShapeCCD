using FK_CLI;
using System;
using System.Collections.Generic;
using FanShapeCCD;

//ウィンドウ変数
fk_AppWindow win = new fk_AppWindow();
WindowSetup();

var model = new fk_Model();
var obbBV = new MyOBB(new fk_Vector(), new fk_Vector(1.0, 0.0, 0.0), new fk_Vector(0.0, 1.0, 0.0), new fk_Vector(0.0, 0.0, 1.0), new fk_Vector(10.0, 10.0, 20.0));
var fanshapeBV = new MyFanShapeBV(new fk_Vector(), new fk_Vector(1.0, 0.0, 0.0), new fk_Vector(0.0, 1.0, 0.0), Math.PI * 120.0 / 180.0, 10.0, 20.0, 5.0);

var block = new fk_Block(10.0, 10.0, 20.0);
var obb = obbBV.GetShape();

model.Shape = block;
model.Material = fk_Material.Yellow;
model.DrawMode = fk_Draw.LINE;
win.Entry(model);

const double ROUGHNESS = 64.0;
const double STEP = 2.0 / ROUGHNESS;
const double START = -1.0;
var fanshapeModel = new fk_Model();
var lineShape = new fk_Line();

for (double t = START; t < 1.0; t += 2.0 / ROUGHNESS)
{
    var a = fanshapeBV.GetPoint(0.0, t, -1.0);
    var b = fanshapeBV.GetPoint(1.0, t, -1.0);
    var c = fanshapeBV.GetPoint(0.0, t, 1.0);
    var d = fanshapeBV.GetPoint(1.0, t, 1.0);

    lineShape.PushLine(a, b);
    lineShape.PushLine(a, c);
    lineShape.PushLine(b, d);
    lineShape.PushLine(c, d);

    if (t != START)
    {
        var pa = fanshapeBV.GetPoint(0.0, t - STEP, -1.0);
        var pb = fanshapeBV.GetPoint(1.0, t - STEP, -1.0);
        var pc = fanshapeBV.GetPoint(0.0, t - STEP, 1.0);
        var pd = fanshapeBV.GetPoint(1.0, t - STEP, 1.0);
        lineShape.PushLine(a, pa);
        lineShape.PushLine(b, pb);
        lineShape.PushLine(c, pc);
        lineShape.PushLine(d, pd);
    }
}

fanshapeModel.Shape = lineShape;
fanshapeModel.LineWidth = 10.0;
fanshapeModel.LineColor = new fk_Color(1.0, 0.0, 0.0);
win.Entry(fanshapeModel);

while (win.Update() == true)
{
    if(win.GetKeyStatus(fk_Key.SHIFT_L))
    {
        model.LoRotateWithVec(new fk_Vector(), fk_Axis.Y, 0.01);
        model.LoRotateWithVec(new fk_Vector(), fk_Axis.X, 0.01);
        model.LoRotateWithVec(new fk_Vector(), fk_Axis.Z, 0.01);
    }
    else if(win.GetKeyStatus(fk_Key.CTRL_L))
    {
        model.LoRotateWithVec(new fk_Vector(), fk_Axis.Y, -0.01);
        model.LoRotateWithVec(new fk_Vector(), fk_Axis.X, -0.01);
        model.LoRotateWithVec(new fk_Vector(), fk_Axis.Z, -0.01);
    }
    obbBV.SyncModel(model);
    fanshapeBV.SyncModel(model);

    lineShape.AllClear();

    for (double t = START; t < 1.0; t += 2.0 / ROUGHNESS)
    {
        var a = fanshapeBV.GetPoint(0.0, t, -1.0);
        var b = fanshapeBV.GetPoint(1.0, t, -1.0);
        var c = fanshapeBV.GetPoint(0.0, t, 1.0);
        var d = fanshapeBV.GetPoint(1.0, t, 1.0);

        lineShape.PushLine(a, b);
        lineShape.PushLine(a, c);
        lineShape.PushLine(b, d);
        lineShape.PushLine(c, d);

        if (t != START)
        {
            var pa = fanshapeBV.GetPoint(0.0, t - STEP, -1.0);
            var pb = fanshapeBV.GetPoint(1.0, t - STEP, -1.0);
            var pc = fanshapeBV.GetPoint(0.0, t - STEP, 1.0);
            var pd = fanshapeBV.GetPoint(1.0, t - STEP, 1.0);
            lineShape.PushLine(a, pa);
            lineShape.PushLine(b, pb);
            lineShape.PushLine(c, pc);
            lineShape.PushLine(d, pd);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////
//関数定義

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