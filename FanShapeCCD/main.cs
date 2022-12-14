using FK_CLI;
using System;
using System.Collections.Generic;
using FanShapeCCD;

//変数宣言
fk_AppWindow win = new fk_AppWindow();
var origin = new fk_Vector();
var fanshapeBV = new MyFanShapeBV(origin, new fk_Vector(0.0, 1.0, 0.0), new fk_Vector(0.0, 0.0, 1.0), Math.PI * 120.0 / 180.0, 10.0, 20.0, 5.0);
var fanshapeModel = new fk_Model();
var ipModel = new fk_Model();
var opModel = new fk_Model();
var insidePoints = new fk_Point();
var outsidePoints = new fk_Point();
var pList = new List<fk_Vector>();
int dispShapeFlg = 1;
int dispPointsFlg = 0;

WindowSetup();

FanShapeSetup(fanshapeModel, fanshapeBV);
win.Entry(fanshapeModel);

PointsSetup(ipModel, opModel, insidePoints, outsidePoints);
win.Entry(ipModel);

LatticeCalc(pList);

while (win.Update() == true)
{
    //モデルの移動・回転
    if (win.GetKeyStatus(fk_Key.TAB)) fanshapeModel.LoRotateWithVec(origin, fk_Axis.X, 0.01);
    if (win.GetKeyStatus(fk_Key.SHIFT_L)) fanshapeModel.LoRotateWithVec(origin, fk_Axis.Y, 0.01);
    if (win.GetKeyStatus(fk_Key.CTRL_L)) fanshapeModel.LoRotateWithVec(origin, fk_Axis.Z, 0.01);
    if (win.GetKeyStatus(fk_Key.UP)) fanshapeModel.LoTranslate(0.0, 0.0, -0.1);
    if (win.GetKeyStatus(fk_Key.DOWN)) fanshapeModel.LoTranslate(0.0, 0.0, 0.1);
    if (win.GetKeyStatus(fk_Key.RIGHT)) fanshapeModel.LoTranslate(0.1, 0.0, 0.0);
    if (win.GetKeyStatus(fk_Key.LEFT)) fanshapeModel.LoTranslate(-0.1, 0.0, 0.0);
    //描画方法の変更
    if (win.GetKeyStatus(fk_Key.SPACE, fk_Switch.DOWN))
    {
        dispShapeFlg++;
        if (dispShapeFlg > 2) dispShapeFlg = 0;

        if (dispShapeFlg == 0) fanshapeModel.DrawMode = fk_Draw.LINE;
        else if(dispShapeFlg == 1) fanshapeModel.DrawMode = fk_Draw.FACE;
        else fanshapeModel.DrawMode = fk_Draw.NONE;
    }
    if(win.GetKeyStatus(fk_Key.ENTER, fk_Switch.DOWN))
    {
        dispPointsFlg++;
        if(dispPointsFlg > 2) dispPointsFlg = 0;

        if(dispPointsFlg == 0) win.Entry(ipModel);
        else if(dispPointsFlg == 1)
        {
            win.Entry(opModel);
            win.Remove(ipModel);
        }
        else win.Remove(opModel);
    }

    fanshapeBV.SyncModel(fanshapeModel);

    insidePoints.AllClear();
    outsidePoints.AllClear();

    foreach (var p in pList)
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
    win.Size = new fk_Dimension(1000, 600);
    //win.ShowGuide(fk_Guide.GRID_XY);
    win.BGColor = new fk_Color(0.6, 0.7, 0.8);
    win.TrackBallMode = true;
    win.Open();
}

void FanShapeSetup(fk_Model fsModel, MyFanShapeBV fsBV)
{
    fsBV.SyncModel(fsModel);
    fsModel.Shape = fsBV.GetShape();
    fsModel.Material = fk_Material.Yellow;
    fsModel.DrawMode = fk_Draw.FACE;
    fsModel.LineWidth = 10.0;
    fsModel.GlVec(new fk_Vector(0.0, 1.0, 0.0));
}

void PointsSetup(fk_Model ipm, fk_Model opm, fk_Point ip, fk_Point op)
{
    ipm.Shape = ip;
    ipm.PointSize = 3.0;
    ipm.PointColor = new fk_Color(1.0, 0.0, 0.0);
    opm.Shape = op;
    opm.PointSize = 3.0;
    opm.PointColor = new fk_Color(0.0, 0.0, 1.0);
}

void LatticeCalc(List<fk_Vector> pl)
{
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
                pl.Add(new fk_Vector(px, py, pz));
            }
        }
    }
}