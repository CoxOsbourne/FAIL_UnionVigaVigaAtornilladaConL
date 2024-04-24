using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures;
using Tekla.Structures.Dialog;
using Tekla.Structures.Dialog.UIControls;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Datatype;
using Tekla.Structures.Catalogs;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Datatype;
using TSG = Tekla.Structures.Geometry3d;

using Connection = Tekla.Structures.Model.Connection;
using ModelObject = Tekla.Structures.Model.ModelObject;
using Tekla.Structures.Model.UI;
using System.Numerics;


namespace WindowsFormsApp1
{
    public partial class Form1 : ApplicationFormBase
    {
        public Form1()
        {
            InitializeComponent();
            base.InitializeForm();
            MyModel = new Model();
        }
        
        private readonly Model MyModel;
        private const double GAP = 10.0; //hueco entre cara y plano medio
        private const double EPSILON = 0.001; //"tolerancia" para decidir si un elemento está pegado a otro
        private double _PlateLength=300;
        //private string _BoltStandard =/*"7990"*/boltCatalogStandard1.SelectedText;

        private int _Class =99;
        private string _ConnectionCode =""/* string.Empty*/;
        private string _AutoDefaults = string.Empty;
        private string _AutoConnection = string.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
            double col_height=double.Parse(textBoxColumnHeight.Text);
            
            if (MyModel.GetConnectionStatus() &&!string.IsNullOrEmpty(textBoxColumnHeight.Text) &&!string.IsNullOrEmpty(textBoxColumnProfile.Text) &&!string.IsNullOrEmpty(textBoxMaterialColumn.Text))
            {
                for (double PositionZ = 0; PositionZ < 2*col_height; PositionZ = PositionZ + col_height)
                {
                    GatherIntel(PositionZ,col_height);
                }
                MyModel.CommitChanges();
            }
        }
        private void GatherIntel(double PositionZ, double col_height)
        {
            string columnProfile = textBoxColumnProfile.Text;
            string columnMaterial = textBoxMaterialColumn.Text;
            Beam column = CreateColumn(PositionZ, col_height, columnProfile,columnMaterial);
        }
        private static Beam CreateColumn(double PositionZ, double column_height, string column_profile, string column_material)
        {
            Beam column = new Beam();
            column.Name = "COLUMN";
            column.Profile.ProfileString = column_profile;
            
            column.Material.MaterialString = column_material;
            column.Class = "2";
            column.StartPoint.X = 0;
            column.StartPoint.Y = 0;
            column.StartPoint.Z = PositionZ;
            column.EndPoint.X = 0;
            column.EndPoint.Y = 0;
            column.EndPoint.Z = PositionZ + column_height;
            column.Position.Rotation = Position.RotationEnum.FRONT;
            column.Position.Plane = Position.PlaneEnum.MIDDLE;
            column.Position.Depth = Position.DepthEnum.MIDDLE;
            if (!column.Insert())
            {
                Console.WriteLine("Insertion of column failed.");
            }

            return column;
        }
        private void profileCatalogColumn_SelectClicked(object sender, EventArgs e)
        {
            profileCatalogColumn.SelectedProfile = textBoxColumnProfile.Text;
        }

        private void profileCatalogColumn_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(textBoxColumnProfile, profileCatalogColumn.SelectedProfile);
        }

        private void materialCatalogColumn_SelectClicked(object sender, EventArgs e)
        {
            materialCatalogColumn.SelectedMaterial=textBoxMaterialColumn.Text;
        }

        private void materialCatalogColumn_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(textBoxMaterialColumn,materialCatalogColumn.SelectedMaterial);
        }

        /*private void splicing_Click(object sender, EventArgs e)
        {

        }*/
        /*private void GatherIntelSplicesAndBolts(Beam beam1, Beam beam2, double PositionZ, double col_height)
        {
            string spliceMaterial = textBoxSpliceMaterial.Text;
            string boltMaterial = textBoxBoltMaterial.Text;
            //CreateSpliceConnection(beam1, beam2);
            //Beam column = CreateColumn(PositionZ, col_height, columnProfile, columnMaterial);
        }*/
        private static bool CheckIfBeamsAreAligned(Beam PrimaryBeam, Beam SecondaryBeam)
        {
            bool result = false;//like a flag variable
            //first we check if there are beams
            if (PrimaryBeam != null && SecondaryBeam != null)
            {
                Line primaryLine = new Line(PrimaryBeam.StartPoint, PrimaryBeam.EndPoint);
                Line secondaryLine = new Line(SecondaryBeam.StartPoint, SecondaryBeam.EndPoint);
                if (TSG.Parallel.LineToLine(primaryLine, secondaryLine))
                    result = true;
            }

            return result;
        }
        private bool CreateRectangularSpliceConnection(Beam PrimaryBeam, Beam SecondaryBeam)
        {
            bool Result = false;
            TransformationPlane originalTransformationPlane = MyModel.GetWorkPlaneHandler().GetCurrentTransformationPlane();
            double webThickness = 0.0;
            double beamHeight = 0.0;
            double flangeThickness = 0.0;
            double innerRoundingRadius = 0.0;
            const double innerMargin = 5.0;

            CoordinateSystem coordSys = PrimaryBeam.GetCoordinateSystem(); //cambio de sistema de coordenadas 
            //comprobar si las vigas están pegadas o no
            if (TSG.Distance.PointToPoint(PrimaryBeam.EndPoint, SecondaryBeam.StartPoint) < EPSILON ||
               TSG.Distance.PointToPoint(PrimaryBeam.EndPoint, SecondaryBeam.EndPoint) < EPSILON)
            {
                coordSys.Origin.Translate(coordSys.AxisX.X, coordSys.AxisX.Y, coordSys.AxisX.Z);
                coordSys.AxisX = -1 * coordSys.AxisX;
            }

            // First get the essential dimensions from the beam
            if (CreateGapBetweenBeams(PrimaryBeam, SecondaryBeam) &&
                SecondaryBeam.GetReportProperty("PROFILE.WEB_THICKNESS", ref webThickness) &&
                SecondaryBeam.GetReportProperty("PROFILE.HEIGHT", ref beamHeight) &&
                SecondaryBeam.GetReportProperty("PROFILE.FLANGE_THICKNESS", ref flangeThickness))
            {
                //Creates plates on both sides of the beam.

                #region Create the Plates

                Beam plate1 = new Beam();
                Beam plate2 = new Beam();
                string spliceMaterial = textBoxSpliceMaterial.Text;

                // And then the optionals if they exist
                SecondaryBeam.GetReportProperty("PROFILE.ROUNDING_RADIUS_1", ref innerRoundingRadius);
                double edgeDistance = (flangeThickness + innerRoundingRadius + innerMargin);
                double plateHeight = beamHeight - 2 * edgeDistance;

                MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(new TransformationPlane(coordSys));

                plate1.Position.Depth = plate2.Position.Depth = Position.DepthEnum.MIDDLE;
                plate1.Position.Rotation = plate2.Position.Rotation = Position.RotationEnum.FRONT;
                plate1.Material.MaterialString = plate2.Material.MaterialString = spliceMaterial;

                plate1.StartPoint = new TSG.Point(-GAP, (-beamHeight / 2.0) + edgeDistance, webThickness);
                plate1.EndPoint = new TSG.Point(plate1.StartPoint.X, (-beamHeight / 2.0) + beamHeight - edgeDistance,
                                            webThickness);
                plate2.StartPoint = new TSG.Point(plate1.StartPoint.X, plate1.StartPoint.Y, -webThickness);
                plate2.EndPoint = new TSG.Point(plate1.EndPoint.X, plate1.EndPoint.Y, -webThickness);

                plate1.Profile.ProfileString = plate2.Profile.ProfileString = "PL" + (int)webThickness + "*" + (int)_PlateLength;
                //plate1.Profile.ProfileString = plate2.Profile.ProfileString = "L100*100*8";
                plate1.Finish = plate2.Finish = "PAINT";

                // With this we help internal code to assign same ID to plates when plugin is modified.
                // To avoid some problems related to links with UDA values or booleans (cuts, fittings) for example.
                plate1.SetLabel("MyPlate01");
                plate2.SetLabel("MyPlate02");

                plate1.Insert();
                plate2.Insert();

                #endregion

                MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(new TransformationPlane(plate1.GetCoordinateSystem()));

                //Creates two boltArrays to connect the plates
                if (CreateBoltArrayRectangularPlate(PrimaryBeam, plate1, plate2, plateHeight, true) &&
                   CreateBoltArrayRectangularPlate(SecondaryBeam, plate1, plate2, plateHeight, false))
                    Result = true;

                MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(originalTransformationPlane);
            }
            return Result;
        }
        private bool CreateLSpliceConnection(Beam PrimaryBeam, Beam SecondaryBeam)
        {
            bool Result = false;
            TransformationPlane originalTransformationPlane = MyModel.GetWorkPlaneHandler().GetCurrentTransformationPlane();
            double webThickness = 0.0;
            double beamHeight = 0.0;
            double flangeThickness = 0.0;
            double innerRoundingRadius = 0.0;
            const double innerMargin = 5.0;
            //double h_plate = 0;

            //double b_plate=0;
            //double t_plate = 0;

            CoordinateSystem coordSys = PrimaryBeam.GetCoordinateSystem(); //cambio de sistema de coordenadas 
            //comprobar si las vigas están pegadas o no
            if (TSG.Distance.PointToPoint(PrimaryBeam.EndPoint, SecondaryBeam.StartPoint) < EPSILON ||
               TSG.Distance.PointToPoint(PrimaryBeam.EndPoint, SecondaryBeam.EndPoint) < EPSILON)
            {
                coordSys.Origin.Translate(coordSys.AxisX.X, coordSys.AxisX.Y, coordSys.AxisX.Z);
                coordSys.AxisX = -1 * coordSys.AxisX;
            }

            // First get the essential dimensions from the beam
            if (CreateGapBetweenBeams(PrimaryBeam, SecondaryBeam) &&
                SecondaryBeam.GetReportProperty("PROFILE.WEB_THICKNESS", ref webThickness) &&
                SecondaryBeam.GetReportProperty("PROFILE.HEIGHT", ref beamHeight) &&
                SecondaryBeam.GetReportProperty("PROFILE.FLANGE_THICKNESS", ref flangeThickness))
            {

                //Creates plates on both sides of the beam.

                #region Create the Plates

                Beam plate1 = new Beam();
                plate1.Name = "Plate1";
                Beam plate2 = new Beam();
                plate2.Name = "Plate2";
                Beam plate3 = new Beam();
                plate3.Name = "Plate3";
                Beam plate4 = new Beam();
                plate4.Name = "Plate4";
                string spliceMaterial = textBoxSpliceMaterial.Text;
                string spliceProfile = textBoxSpliceProfile.Text;
                //contar asteriscos, hay perfiles en L donde h=b y otros donde h!=b
                int countAsterisks = spliceProfile.Count(ch => ch =='*');
                plate1.Material.MaterialString = plate2.Material.MaterialString = plate3.Material.MaterialString = plate4.Material.MaterialString = spliceMaterial;
                plate1.Profile.ProfileString = plate2.Profile.ProfileString = plate3.Profile.ProfileString = plate4.Profile.ProfileString = spliceProfile;
                plate1.Finish = plate2.Finish = plate3.Finish = plate4.Finish = "PAINT";
                //coger los datos del perfil en L de forma un poco guarra

                string[] spliceProfileProperties = spliceProfile.Split('*');
                if(countAsterisks==1)
                {
                    string splice_h = spliceProfileProperties[0];
                    string splice_t = spliceProfileProperties[1];
                    string splice_h_no_chars = Regex.Match(splice_h, @"\d+").Value; 
                    double d_splice_h = Convert.ToDouble(splice_h_no_chars);
                    double d_splice_b = d_splice_h;
                    double d_splice_t = Convert.ToDouble(splice_t);
                    // And then the optionals if they exist
                    SecondaryBeam.GetReportProperty("PROFILE.ROUNDING_RADIUS_1", ref innerRoundingRadius);
                    double edgeDistance = (flangeThickness + innerRoundingRadius + innerMargin);
                    double plateHeight = beamHeight - 2 * edgeDistance;
                    MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(new TransformationPlane(coordSys));
                    plate1.Position.Depth = plate2.Position.Depth =plate3.Position.Depth= plate4.Position.Depth = Position.DepthEnum.MIDDLE;
                    plate1.Position.Rotation = plate2.Position.Rotation =/*plate3.Position.Rotation=plate4.Position.Rotation= */Position.RotationEnum.FRONT;
                    //Sin offsets, a ver...
                    plate3.Position.Rotation = Position.RotationEnum.BACK;
                    plate4.Position.Rotation = Position.RotationEnum.BACK;
                    plate1.StartPoint = new TSG.Point((-d_splice_h / 2) - (GAP), (-beamHeight / 2.0) + edgeDistance, (d_splice_b / 2) + (webThickness / 2));
                    plate1.EndPoint = new TSG.Point(plate1.StartPoint.X, (-beamHeight / 2.0) + beamHeight - edgeDistance, plate1.StartPoint.Z);
                    plate1.Insert();
                    plate2.StartPoint = new TSG.Point(-plate1.StartPoint.X, plate1.EndPoint.Y, plate1.StartPoint.Z);
                    plate2.EndPoint = new TSG.Point(plate2.StartPoint.X, plate1.StartPoint.Y, plate1.StartPoint.Z);
                    plate2.Insert();
                    plate3.StartPoint = new TSG.Point(plate1.StartPoint.X, -plate1.StartPoint.Y, -plate1.StartPoint.Z);
                    plate3.EndPoint = new TSG.Point(plate3.StartPoint.X, -plate1.EndPoint.Y, plate3.StartPoint.Z);
                    plate3.Insert();
                    plate4.StartPoint = new TSG.Point(plate2.StartPoint.X, -plate2.StartPoint.Y, plate3.StartPoint.Z);
                    plate4.EndPoint = new TSG.Point(plate2.StartPoint.X, -plate2.EndPoint.Y, plate4.StartPoint.Z);
                    plate4.Insert();
                    //Creates two boltArrays to connect the plates
                    if (CreateBoltArrayLPlate(PrimaryBeam, plate1, plate2, plate3, plate4,d_splice_h, d_splice_b,d_splice_t,plateHeight, true) &&
                     CreateBoltArrayLPlate(SecondaryBeam, plate1, plate2, plate3, plate4,d_splice_h, d_splice_b,d_splice_t,plateHeight, false))
                    {
                        Result = true;
                    }
                }
                else
                {
                    if(countAsterisks==2)
                    {
                        string splice_h = spliceProfileProperties[0];
                        string splice_b = spliceProfileProperties[1];
                        string splice_t = spliceProfileProperties[2];
                        string splice_h_no_chars = Regex.Match(splice_h, @"\d+").Value;

                        double d_splice_h = Convert.ToDouble(splice_h_no_chars);
                        
                        double d_splice_b = Convert.ToDouble(splice_b);
                        double d_splice_t = Convert.ToDouble(splice_t);
                        // And then the optionals if they exist
                        SecondaryBeam.GetReportProperty("PROFILE.ROUNDING_RADIUS_1", ref innerRoundingRadius);
                        double edgeDistance = (flangeThickness + innerRoundingRadius + innerMargin);
                        double plateHeight = beamHeight - 2 * edgeDistance;
                        MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(new TransformationPlane(coordSys));
                        plate1.Position.Depth = plate2.Position.Depth = plate3.Position.Depth = plate4.Position.Depth = Position.DepthEnum.MIDDLE;
                        plate1.Position.Rotation = plate2.Position.Rotation =/*plate3.Position.Rotation=plate4.Position.Rotation= */Position.RotationEnum.FRONT;
                        //Sin offsets, a ver...
                        plate3.Position.Rotation = Position.RotationEnum.BACK;
                        plate4.Position.Rotation = Position.RotationEnum.BACK;
                        plate1.StartPoint = new TSG.Point((-d_splice_h / 2) - (GAP), (-beamHeight / 2.0) + edgeDistance, (d_splice_b / 2) + (webThickness / 2));
                        plate1.EndPoint = new TSG.Point(plate1.StartPoint.X, (-beamHeight / 2.0) + beamHeight - edgeDistance, plate1.StartPoint.Z);
                        plate1.Insert();
                        plate2.StartPoint = new TSG.Point(-plate1.StartPoint.X, plate1.EndPoint.Y, plate1.StartPoint.Z);
                        plate2.EndPoint = new TSG.Point(plate2.StartPoint.X, plate1.StartPoint.Y, plate1.StartPoint.Z);
                        plate2.Insert();
                        plate3.StartPoint = new TSG.Point(plate1.StartPoint.X, -plate1.StartPoint.Y, -plate1.StartPoint.Z);
                        plate3.EndPoint = new TSG.Point(plate3.StartPoint.X, -plate1.EndPoint.Y, plate3.StartPoint.Z);
                        plate3.Insert();
                        plate4.StartPoint = new TSG.Point(plate2.StartPoint.X, -plate2.StartPoint.Y, plate3.StartPoint.Z);
                        plate4.EndPoint = new TSG.Point(plate2.StartPoint.X, -plate2.EndPoint.Y, plate4.StartPoint.Z);
                        plate4.Insert();
                        if (CreateBoltArrayLPlate(PrimaryBeam, plate1, plate2, plate3, plate4,d_splice_h, d_splice_b, d_splice_t, plateHeight, true) &&
                     CreateBoltArrayLPlate(SecondaryBeam, plate1, plate2, plate3, plate4, d_splice_h, d_splice_b, d_splice_t, plateHeight, false))
                        {
                            Result = true;
                        }
                    }
                }    
                //int countAsterisks = spliceProfileProperties.Count(f => f == "*");
                //string splice_h = spliceProfileProperties[0];
                //string splice_b = spliceProfileProperties[1];
                //string splice_t = spliceProfileProperties[2];
                //string splice_h_only_numbers = string.Empty;
                //var matchL = Regex.Matches(splice_h, @"\d+");
                //foreach (var match in matchL)
                //{
                //    splice_h_only_numbers = splice_h_only_numbers + match;
                //}
                //double d_splice_h = Convert.ToDouble(splice_h_only_numbers);
                //double d_splice_b = Convert.ToDouble(splice_b);
                //double d_splice_t = Convert.ToDouble(splice_t);
                // And then the optionals if they exist
                //SecondaryBeam.GetReportProperty("PROFILE.ROUNDING_RADIUS_1", ref innerRoundingRadius);
                //double edgeDistance = (flangeThickness + innerRoundingRadius + innerMargin);
                //double plateHeight = beamHeight - 2 * edgeDistance;
                //MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(new TransformationPlane(coordSys));
                //plate1.Position.Depth = plate2.Position.Depth =plate3.Position.Depth= plate4.Position.Depth = Position.DepthEnum.MIDDLE;
                //plate1.Position.Rotation = plate2.Position.Rotation =/*plate3.Position.Rotation=plate4.Position.Rotation= */Position.RotationEnum.FRONT;
                //plate1.Profile.ProfileString = plate2.Profile.ProfileString = plate3.Profile.ProfileString = plate4.Profile.ProfileString = "L100*65*11";
                //plate1.Material.MaterialString = plate2.Material.MaterialString = plate3.Material.MaterialString = plate4.Material.MaterialString = "S355JR";
                
                /*plate2.Position.RotationOffset = 180; //funciona
                plate3.Position.RotationOffset = 180; //funciona
                
               
                //Funcionan
                plate1.StartPoint = new TSG.Point((-50/2)-(2.5*GAP), (-beamHeight / 2.0) + edgeDistance,32.5+(webThickness/2));
                plate1.EndPoint = new TSG.Point(plate1.StartPoint.X, (-beamHeight / 2.0) + beamHeight - edgeDistance, 32.5 + (webThickness / 2));
                plate2.StartPoint = new TSG.Point(plate1.StartPoint.X, (-beamHeight / 2.0) + beamHeight - edgeDistance, -32.5+(-webThickness/2));
                plate2.EndPoint = new TSG.Point(plate1.EndPoint.X, (-beamHeight / 2.0) + edgeDistance, plate2.StartPoint.Z);

                plate3.StartPoint = new TSG.Point((50 / 2) + (2.5 * GAP), plate1.StartPoint.Y, -32.5 -(webThickness / 2));
                plate3.EndPoint= new TSG.Point(plate3.StartPoint.X,plate1.EndPoint.Y, plate3.StartPoint.Z);

                plate4.StartPoint = new TSG.Point((50 / 2) + (2.5 * GAP), plate1.EndPoint.Y, +32.5 + (webThickness / 2));
                plate4.EndPoint = new TSG.Point(plate4.StartPoint.X, plate1.StartPoint.Y, plate4.StartPoint.Z);*/

                //Sin offsets, a ver...
                //plate3.Position.Rotation=Position.RotationEnum.BACK;
                //plate4.Position.Rotation = Position.RotationEnum.BACK;
                //plate1.StartPoint = new TSG.Point((-d_splice_h/2) -  ( GAP), (-beamHeight / 2.0) + edgeDistance, (d_splice_b/2) + (webThickness / 2));
                //plate1.EndPoint = new TSG.Point(plate1.StartPoint.X, (-beamHeight / 2.0) + beamHeight - edgeDistance, plate1.StartPoint.Z);
                //plate1.Insert();
                //plate2.StartPoint = new TSG.Point(-plate1.StartPoint.X, plate1.EndPoint.Y, plate1.StartPoint.Z);
                //plate2.EndPoint = new TSG.Point(plate2.StartPoint.X, plate1.StartPoint.Y, plate1.StartPoint.Z);
                //plate2.Insert();
                //plate3.StartPoint = new TSG.Point(plate1.StartPoint.X,-plate1.StartPoint.Y, -plate1.StartPoint.Z);
                //plate3.EndPoint = new TSG.Point(plate3.StartPoint.X, -plate1.EndPoint.Y, plate3.StartPoint.Z);
                //plate3.Insert();
                //plate4.StartPoint = new TSG.Point(plate2.StartPoint.X, -plate2.StartPoint.Y, plate3.StartPoint.Z);
                //plate4.EndPoint = new TSG.Point(plate2.StartPoint.X, -plate2.EndPoint.Y, plate4.StartPoint.Z);
                //plate4.Insert();

                /*
                plate4.StartPoint = new TSG.Point((50 / 2) + (2.5 * GAP), plate1.EndPoint.Y, +32.5 + (webThickness / 2));
                plate4.EndPoint = new TSG.Point(plate4.StartPoint.X, plate1.StartPoint.Y, plate4.StartPoint.Z);
                plate4.Insert();*/
                //perfil y material
                //plate1.Profile.ProfileString = plate2.Profile.ProfileString = "PL" + (int)webThickness + "*" + (int)_PlateLength;


                // With this we help internal code to assign same ID to plates when plugin is modified.
                // To avoid some problems related to links with UDA values or booleans (cuts, fittings) for example.
                plate1.SetLabel("MyPlate01");
                plate2.SetLabel("MyPlate02");
                plate3.SetLabel("MyPlate03");
                plate4.SetLabel("MyPlate04");
                /*
                //insert plates
                plate1.Insert();
                plate2.Insert();
                plate3.Insert();
                plate4.Insert();*/
                #endregion
                
                MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(new TransformationPlane(plate1.GetCoordinateSystem()));

                //Creates two boltArrays to connect the plates
                //if (CreateBoltArray(PrimaryBeam, plate1, plate2, , true) &&
                  // CreateBoltArray(SecondaryBeam, plate1, plate2, plateHeight, false))
                //{
                //    Result = true;
                //}
                    

                MyModel.GetWorkPlaneHandler().SetCurrentTransformationPlane(originalTransformationPlane);
            }
            return Result;
        }
        private static bool CreateGapBetweenBeams(Beam PrimaryBeam, Beam SecondaryBeam)
        {
            bool result = false;

            if (PrimaryBeam != null && SecondaryBeam != null)
            {
                //Get vectors defined by the beams, to move their extremes along them when creating the gaps
                TSG.Vector primaryBeamVector = new TSG.Vector(PrimaryBeam.EndPoint.X - PrimaryBeam.StartPoint.X,
                                                      PrimaryBeam.EndPoint.Y - PrimaryBeam.StartPoint.Y,
                                                      PrimaryBeam.EndPoint.Z - PrimaryBeam.StartPoint.Z);
                primaryBeamVector.Normalize(GAP);
                TSG.Vector secondaryBeamVector = new TSG.Vector(SecondaryBeam.EndPoint.X - SecondaryBeam.StartPoint.X,
                                                        SecondaryBeam.EndPoint.Y - SecondaryBeam.StartPoint.Y,
                                                        SecondaryBeam.EndPoint.Z - SecondaryBeam.StartPoint.Z);
                secondaryBeamVector.Normalize(GAP);

                TSG.Point PrimaryBeamEdge;
                TSG.Point SecondaryBeamEdge;

                if (PrimaryBeam.StartPoint == SecondaryBeam.StartPoint)
                {
                    PrimaryBeamEdge = PrimaryBeam.StartPoint;
                    SecondaryBeamEdge = SecondaryBeam.StartPoint;

                    if (CreateFittings(PrimaryBeam, SecondaryBeam, PrimaryBeamEdge, SecondaryBeamEdge,
                                      primaryBeamVector, secondaryBeamVector))
                        result = true;
                }
                else if (PrimaryBeam.EndPoint == SecondaryBeam.StartPoint)
                {
                    ChangeVectorDirection(primaryBeamVector);

                    PrimaryBeamEdge = PrimaryBeam.EndPoint;
                    SecondaryBeamEdge = SecondaryBeam.StartPoint;

                    if (CreateFittings(PrimaryBeam, SecondaryBeam, PrimaryBeamEdge, SecondaryBeamEdge,
                                      primaryBeamVector, secondaryBeamVector))
                        result = true;
                }
                else if (PrimaryBeam.StartPoint == SecondaryBeam.EndPoint)
                {
                    ChangeVectorDirection(secondaryBeamVector);

                    PrimaryBeamEdge = PrimaryBeam.StartPoint;
                    SecondaryBeamEdge = SecondaryBeam.EndPoint;

                    if (CreateFittings(PrimaryBeam, SecondaryBeam, PrimaryBeamEdge, SecondaryBeamEdge,
                                      primaryBeamVector, secondaryBeamVector))
                        result = true;
                }
                else if (PrimaryBeam.EndPoint == SecondaryBeam.EndPoint)
                {
                    ChangeVectorDirection(primaryBeamVector);
                    ChangeVectorDirection(secondaryBeamVector);

                    PrimaryBeamEdge = PrimaryBeam.EndPoint;
                    SecondaryBeamEdge = SecondaryBeam.EndPoint;

                    if (CreateFittings(PrimaryBeam, SecondaryBeam, PrimaryBeamEdge, SecondaryBeamEdge,
                                      primaryBeamVector, secondaryBeamVector))
                        result = true;
                }
            }

            return result;
        }
        /*private string GetBoltStandard(string boltStandard)
        {
            string boltMaterial;
            boltMaterial = boltCatalogStandard1.SelectedText;
            return boltMaterial;
        }*/
        //Rectangular bolt array
        private bool CreateBoltArrayRectangularPlate(Beam beam, Beam plate1, Beam plate2, double plateHeight, bool Primary/*, string boltStandard*/)
        {
            bool result = false;
            string boltMaterial = string.Empty;
            BoltArray B = new BoltArray();
            //string boltMaterial;
            string boltStandard=textBoxBoltStandard.Text;
            double boltSize=Convert.ToDouble(textBoxBoltSize.Text);
            //boltStandard = GetBoltStandard(boltMaterial);
            //B.BoltStandard = boltStandard;

            B.PartToBoltTo = plate1;
            B.PartToBeBolted = beam;
            //B.AddOtherPartToBolt(plate2);
            

            if (Primary)
            {
                B.FirstPosition = new TSG.Point(plateHeight / 2.0, _PlateLength / 2.0, 0.0);
                B.SecondPosition = new TSG.Point(plateHeight / 2.0, -_PlateLength / 2.0, 0.0);
            }
            else
            {
                B.FirstPosition = new TSG.Point(plateHeight / 2.0, -_PlateLength / 2.0, 0.0);
                B.SecondPosition = new TSG.Point(plateHeight / 2.0, _PlateLength / 2.0, 0.0);
            }

            B.StartPointOffset.Dx = B.EndPointOffset.Dx = _PlateLength - 75/*90*/;
            B.StartPointOffset.Dy = B.EndPointOffset.Dy = 0;
            B.StartPointOffset.Dz = B.EndPointOffset.Dz = 0;

            B.BoltSize = 20;
            B.Tolerance = 2.00;
            //B.BoltStandard = _BoltStandard.selectedStandard;
            B.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_SITE;
            B.CutLength = 105;

            B.Length = 60;
            B.ExtraLength = 0;
            B.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_YES;

            B.Position.Depth = Position.DepthEnum.MIDDLE;
            B.Position.Plane = Position.PlaneEnum.MIDDLE;
            B.Position.Rotation = Position.RotationEnum.FRONT;

            B.Bolt = true;
            B.Washer1 = false;
            B.Washer2 = B.Washer3 = true;
            B.Nut1 = true;
            B.Nut2 = false;
            B.Hole1 = B.Hole2 = B.Hole3 = B.Hole4 = B.Hole5 = false;

            B.AddBoltDistX(0.0);
            //Mover tornillos a lo ancho de la chapa
            B.AddBoltDistY(plateHeight - 150.0 /*100*/);

            if (B.Insert())
                result = true;

            return result;
        }
        //L profile plate bolt array
        private bool CreateBoltArrayLPlate(Beam beam,Beam plate1, Beam plate2, Beam plate3, Beam plate4, double plateLength, double plateWidth, double plateThickness,double plateLong, bool Primary)
        {
            double beamHeight = 0;
            beam.GetReportProperty("PROFILE.HEIGHT", ref beamHeight);
            bool result = false;
            string boltMaterial = string.Empty;
            BoltArray B = new BoltArray();
            //string boltMaterial;
            string boltStandard = textBoxBoltStandard.Text;
            double boltSize = Convert.ToDouble(textBoxBoltSize.Text);
            //boltStandard = GetBoltStandard(boltMaterial);
            //B.BoltStandard = boltStandard;

            B.PartToBoltTo = plate1;
            B.PartToBoltTo = plate3;
            B.PartToBeBolted = plate2;
            B.PartToBeBolted = plate4;
            //B.Position.Rotation = Position.RotationEnum.FRONT;
            //B.Position.RotationOffset = 90;

            //B.AddOtherPartToBolt(plate2);

            if (Primary)
            {
                B.FirstPosition = new TSG.Point(0, 0,0);
                B.SecondPosition = new TSG.Point(plateThickness, 0, 0);
                B.Position.Depth = Position.DepthEnum.MIDDLE;
                B.Position.Plane = Position.PlaneEnum.MIDDLE;
                B.Position.Rotation = Position.RotationEnum.FRONT;
                B.Position.RotationOffset = 90;
            }
            else
            {
                B.FirstPosition = new TSG.Point(0, 0, 0);
                B.SecondPosition = new TSG.Point(plateThickness, 0, 0);
                B.Position.Depth = Position.DepthEnum.MIDDLE;
                B.Position.Plane = Position.PlaneEnum.MIDDLE;
                B.Position.Rotation = Position.RotationEnum.FRONT;
                B.Position.RotationOffset = 90;
            }
            /*if (Primary)
            {
                B.FirstPosition = new TSG.Point((plateLength / 2.0) + plateThickness, plateLong / 2.0,2*(GAP+plateThickness) 0.0);
                B.SecondPosition = new TSG.Point((plateLength / 2.0) + plateThickness, -plateLong / 2.0, 2 * (GAP + plateThickness)0.0);
                B.Position.Depth = Position.DepthEnum.MIDDLE;
                B.Position.Plane = Position.PlaneEnum.MIDDLE;
                B.Position.Rotation = Position.RotationEnum.FRONT;
                B.Position.RotationOffset = 90;
            }
            else
            {
                B.FirstPosition = new TSG.Point((plateLength / 2.0)+plateThickness, -plateLong / 2.0, 2 * (GAP + plateThickness)0.0);
                B.SecondPosition = new TSG.Point((plateLength / 2.0)+plateThickness, plateLong / 2.0, 2 * (GAP + plateThickness)0.0);
                B.Position.Depth = Position.DepthEnum.MIDDLE;
                B.Position.Plane = Position.PlaneEnum.MIDDLE;
                B.Position.Rotation = Position.RotationEnum.FRONT;
                B.Position.RotationOffset = 90;
            }*/

            //B.StartPointOffset.Dx = B.EndPointOffset.Dx = _PlateLength - 75/*90*/;
            //B.StartPointOffset.Dy = B.EndPointOffset.Dy = 0;
            //B.StartPointOffset.Dz = B.EndPointOffset.Dz = 2*(GAP+(2*plateThickness)+(plateLength/2));

            //B.BoltSize = 20;
            B.Tolerance = 2.00;
            //B.BoltStandard = _BoltStandard.selectedStandard;
            B.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_SITE;
            B.CutLength = 105;

            B.Length = 60;
            B.ExtraLength = 0;
            B.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_YES;

            /*B.Position.Depth = Position.DepthEnum.MIDDLE;
            B.Position.Plane = Position.PlaneEnum.MIDDLE;
            B.Position.Rotation = Position.RotationEnum.FRONT;*/

            B.Bolt = true;
            B.Washer1 = false;
            B.Washer2 = B.Washer3 = true;
            B.Nut1 = true;
            B.Nut2 = false;
            B.Hole1 = B.Hole2 = B.Hole3 = B.Hole4/* = B.Hole5 */= false;

            B.AddBoltDistX((2*GAP)+plateThickness);
            //Mover tornillos a lo ancho de la chapa
            B.AddBoltDistY(GAP+(plateLength/4) /*-plateLong/4  150.0 100*/);
            


            if (B.Insert())
                result = true;

            return result;
        }
        private static bool CreateFittings(Beam PrimaryBeam, Beam SecondaryBeam, TSG.Point PrimaryBeamEdge, TSG.Point SecondaryBeamEdge,
                                           Tekla.Structures.Geometry3d.Vector primaryBeamVector, Tekla.Structures.Geometry3d.Vector secondaryBeamVector)
        {
            bool Result = false;
            Fitting fitPrimary = new Fitting();
            Fitting fitSecondary = new Fitting();
            CoordinateSystem PrimaryCoordinateSystem = PrimaryBeam.GetCoordinateSystem();
            CoordinateSystem SecondaryCoordinateSystem = PrimaryBeam.GetCoordinateSystem();

            PrimaryBeamEdge.Translate(primaryBeamVector.X, primaryBeamVector.Y, primaryBeamVector.Z);
            SecondaryBeamEdge.Translate(secondaryBeamVector.X, secondaryBeamVector.Y, secondaryBeamVector.Z);

            fitPrimary.Plane = new TSM.Plane();
            fitPrimary.Plane.Origin = new TSG.Point(PrimaryBeamEdge.X, PrimaryBeamEdge.Y, PrimaryBeamEdge.Z);
            fitPrimary.Plane.AxisX = new TSG.Vector(TSG.Vector.Cross(PrimaryCoordinateSystem.AxisX,
                                                TSG.Vector.Cross(PrimaryCoordinateSystem.AxisX, PrimaryCoordinateSystem.AxisY)));
            fitPrimary.Plane.AxisX.Normalize(500);
            fitPrimary.Plane.AxisY = new TSG.Vector(TSG.Vector.Cross(PrimaryCoordinateSystem.AxisX, PrimaryCoordinateSystem.AxisY));
            fitPrimary.Plane.AxisY.Normalize(500);
            fitPrimary.Father = PrimaryBeam;

            fitSecondary.Plane = new TSM.Plane();
            fitSecondary.Plane.Origin = new TSG.Point(SecondaryBeamEdge.X, SecondaryBeamEdge.Y, SecondaryBeamEdge.Z);
            fitSecondary.Plane.AxisX = new TSG.Vector(TSG.Vector.Cross(SecondaryCoordinateSystem.AxisX,
                                                TSG.Vector.Cross(SecondaryCoordinateSystem.AxisX, SecondaryCoordinateSystem.AxisY)));
            fitSecondary.Plane.AxisX.Normalize(500);
            fitSecondary.Plane.AxisY = new TSG.Vector(TSG.Vector.Cross(SecondaryCoordinateSystem.AxisX, SecondaryCoordinateSystem.AxisY));
            fitSecondary.Plane.AxisY.Normalize(500);
            fitSecondary.Father = SecondaryBeam;

            if (fitPrimary.Insert() && fitSecondary.Insert())
                Result = true;

            return Result;
        }
        private static void ChangeVectorDirection(TSG.Point vector)
        {
            vector.X = -1 * vector.X;
            vector.Y = -1 * vector.Y;
            vector.Z = -1 * vector.Z;
        }

        private void materialCatalogSplice_SelectClicked(object sender, EventArgs e)
        {
            materialCatalogSplice.SelectedMaterial = textBoxSpliceMaterial.Text;
        }

        private void materialCatalogSplice_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(textBoxSpliceMaterial,materialCatalogSplice.SelectedMaterial);
        }



        

        /*private void boltCatalogStandard1_SelectedValueChanged(object sender, EventArgs e)
        {
            SetAttributeValue(boltCatalogStandard1, boltCatalogStandard1.SelectedText);
        }*/

        private void rectangular_splicing_Click(object sender, EventArgs e)
        {
            if (MyModel.GetConnectionStatus())
            {
                TSM.UI.Picker Picker = new TSM.UI.Picker();
                Beam beam1 = Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Beam;
                Beam beam2 = Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Beam;
                bool Result = false;
                if (beam1 != null && beam2 != null)
                {
                    string beam1ProfileType = "";
                    string beam2ProfileType = "";
                    beam1.GetReportProperty("PROFILE_TYPE", ref beam1ProfileType);
                    beam2.GetReportProperty("PROFILE_TYPE", ref beam2ProfileType);
                    if (beam1.Profile.ProfileString == beam2.Profile.ProfileString &&
                       beam1ProfileType == beam2ProfileType)
                    {
                        if (CheckIfBeamsAreAligned(beam1, beam2))
                        {
                            if (CreateRectangularSpliceConnection(beam1, beam2))
                            {

                                Result = true;
                            }

                        }
                    }
                }
            }
            MyModel.CommitChanges();
        }

        private void LSplicing_Click(object sender, EventArgs e)
        {
            if (MyModel.GetConnectionStatus())
            {
                TSM.UI.Picker Picker = new TSM.UI.Picker();
                Beam beam1 = Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Beam;
                Beam beam2 = Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Beam;
                bool Result = false;
                if (beam1 != null && beam2 != null)
                {
                    string beam1ProfileType = "";
                    string beam2ProfileType = "";
                    beam1.GetReportProperty("PROFILE_TYPE", ref beam1ProfileType);
                    beam2.GetReportProperty("PROFILE_TYPE", ref beam2ProfileType);
                    if (beam1.Profile.ProfileString == beam2.Profile.ProfileString &&
                       beam1ProfileType == beam2ProfileType)
                    {
                        if (CheckIfBeamsAreAligned(beam1, beam2))
                        {
                            if (CreateLSpliceConnection(beam1, beam2))
                            {

                                Result = true;
                            }

                        }
                    }
                }
            }
            MyModel.CommitChanges();
        }

        private void profileCatalogSplice_SelectClicked(object sender, EventArgs e)
        {
            profileCatalogSplice.SelectedProfile = textBoxSpliceProfile.Text;
        }

        private void profileCatalogSplice_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(textBoxSpliceProfile, profileCatalogSplice.SelectedProfile);
        }

        
    }
}
