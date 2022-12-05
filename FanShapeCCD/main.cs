using FK_CLI;
using System;
using FanShapeCCD;
using System.Collections.Generic;

//ウィンドウ変数
fk_AppWindow win = new fk_AppWindow();
WindowSetup();

var origin = new fk_Vector();
const double AXISLENGTH = 30.0;

fk_Model[] axisModels = new fk_Model[3];
fk_Line[] axisLines = new fk_Line[3];

for(int i=0; i<3; i++)
{
    axisModels[i] = new fk_Model();
    axisLines[i] = new fk_Line();
    axisModels[i].Shape = axisLines[i];
    axisModels[i].LineWidth = 1.0;

    win.Entry(axisModels[i]);
}

axisModels[0].LineColor = new fk_Color(1.0, 0.0, 0.0);
axisModels[1].LineColor = new fk_Color(0.0, 1.0, 0.0);
axisModels[2].LineColor = new fk_Color(0.0, 0.0, 1.0);

var fanshapeBV = new MyFanShapeBV(origin, new fk_Vector(0.0, 1.0, 0.0), new fk_Vector(0.0, 0.0, 1.0), Math.PI * 120.0 / 180.0, 10.0, 20.0, 5.0);

var fanshapeModel = new fk_Model();
fanshapeBV.SyncModel(fanshapeModel);
fanshapeModel.Shape = fanshapeBV.GetShape();
fanshapeModel.Material = fk_Material.Yellow;
fanshapeModel.DrawMode = fk_Draw.FACE;
fanshapeModel.LineWidth = 10.0;
fanshapeModel.GlVec(new fk_Vector(0.0, 1.0, 0.0));
win.Entry(fanshapeModel);



var ipModel = new fk_Model();
var opModel = new fk_Model();
var insidePoints = new fk_Point();
var outsidePoints = new fk_Point();
ipModel.Shape = insidePoints;
ipModel.PointSize = 3.0;
ipModel.PointColor = new fk_Color(1.0, 0.0, 0.0);
opModel.Shape = outsidePoints;
opModel.PointSize = 3.0;
opModel.PointColor = new fk_Color(0.0, 0.0, 1.0);
win.Entry(opModel);

var pArray = new List<fk_Vector>();
const int NUMS = 25;
const double RANGE = 50.0;
const double STEP = RANGE / NUMS;
const double START = (RANGE - STEP) / 2.0;

for (int x = 0; x < NUMS; x++)
{
    double px = STEP * x - START;
    for (int y = 0; y < NUMS; y++)
    {
        double py = STEP * y - START;
        for (int z = 0; z < NUMS; z++)
        {
            double pz = STEP * z - START;
            pArray.Add(new fk_Vector(px, py, pz));
        }
    }
}

bool dispShape = false;
bool dispPoints = false;

while (win.Update() == true)
{
    if(win.GetKeyStatus(fk_Key.TAB)) fanshapeModel.LoRotateWithVec(origin, fk_Axis.X, 0.01);
    if (win.GetKeyStatus(fk_Key.SHIFT_L)) fanshapeModel.LoRotateWithVec(origin, fk_Axis.Y, 0.01);
    if (win.GetKeyStatus(fk_Key.CTRL_L)) fanshapeModel.LoRotateWithVec(origin, fk_Axis.Z, 0.01);
    if (win.GetKeyStatus(fk_Key.SPACE, fk_Switch.DOWN))
    {
        dispShape = !dispShape;
        if (dispShape) fanshapeModel.DrawMode = fk_Draw.LINE;
        else fanshapeModel.DrawMode = fk_Draw.FACE;
    }
    if(win.GetKeyStatus(fk_Key.ENTER, fk_Switch.DOWN))
    {
        dispPoints = !dispPoints;
        if(dispPoints)
        {
            win.Entry(ipModel);
            win.Remove(opModel);
        }
        else
        {
            win.Entry(opModel);
            win.Remove(ipModel);
        }
    }

    for (int i = 0; i < 3; i++) axisLines[i].AllClear();
    axisLines[0].PushLine(origin, fanshapeModel.Vec ^ fanshapeModel.Upvec * AXISLENGTH);
    axisLines[1].PushLine(origin, fanshapeModel.Upvec * AXISLENGTH);
    axisLines[2].PushLine(origin, fanshapeModel.Vec * AXISLENGTH);

    fanshapeBV.SyncModel(fanshapeModel);

    insidePoints.AllClear();
    outsidePoints.AllClear();

    foreach (var p in pArray)
    {
        if (fanshapeBV.PointInOutCheck(p))
            insidePoints.PushVertex(p);
        else outsidePoints.PushVertex(p);
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
    //win.ShowGuide(fk_Guide.GRID_XY);
    win.BGColor = new fk_Color(0.6, 0.7, 0.8);
    win.TrackBallMode = true;
    win.Open();
}