using FK_CLI;
using System;
using System.Collections.Generic;
using FanShapeCCD;

//ウィンドウ変数
fk_AppWindow win = new fk_AppWindow();
WindowSetup();

var model = new fk_Model();
var obbBV = new MyOBB(new fk_Vector(), new fk_Vector(1.0, 0.0, 0.0), new fk_Vector(0.0, 1.0, 0.0), new fk_Vector(0.0, 0.0, 1.0), new fk_Vector(10.0, 10.0, 20.0));
var fanshapeBV = new MyFanShapeBV(new fk_Vector(), new fk_Vector(1.0, 0.0, 0.0), new fk_Vector(0.0, 1.0, 0.0), Math.PI * 60.0 / 180.0, 10.0, 20.0, 5.0);

var block = new fk_Block(10.0, 10.0, 20.0);
var obb = obbBV.GetShape();

model.Shape = block;
model.Material = fk_Material.Yellow;
model.DrawMode = fk_Draw.LINE;
win.Entry(model);

const int POINTNUM = 1000;
var pointModel = new fk_Model[POINTNUM];
var point = new fk_Point[POINTNUM];
var pointArray = new fk_Vector[POINTNUM];

for(int i = 0; i < 10; i++)
{
    double x = i * 5.0 - 22.5;
    for(int j = 0; j < 10; j++)
    {
        double y = j * 5.0 - 22.5;
        for(int k = 0; k < 10; k++)
        {
            double z = k * 5.0 - 22.5;
            int num = i * 100 + j * 10 + k;

            pointArray[num] = new fk_Vector(x, y, z);
            point[num] = new fk_Point();
            point[num].PushVertex(pointArray[num]);

            pointModel[num] = new fk_Model();
            pointModel[num].Shape = point[num];
            pointModel[num].PointSize = 10.0;
            win.Entry(pointModel[num]);
        }
    }
}

int count = 0;
var red = new fk_Color(1.0, 0.0, 0.0);
var black = new fk_Color(0.0, 0.0, 0.0);

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

    for(int i = 0; i < pointArray.Length; i++)
    {
        if (fanshapeBV.PointInOutCheck(pointArray[i])) pointModel[i].PointColor = red;
        else pointModel[i].PointColor = black;
    }

    //if ((count++ / 120) % 2 == 0) model.Shape = obb;
    //else model.Shape = block;
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