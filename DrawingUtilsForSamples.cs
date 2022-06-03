using System;
using System.Windows.Media;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;

namespace NodeMapper
{
    public class DrawingUtilsForSamples
    {
        public static float EllipseSweepAngle(Ellipse el) {
            return (float)((el.ParEnd - el.ParStart) / Math.PI * 180);
        }

        public static float EllipseStartAngle(Ellipse el) {
            return (float)(el.ParStart / Math.PI * 180);
        }

        public static void AddNode(string id, GeometryGraph graph, double w, double h) {
            graph.Nodes.Add(new Node(CreateCurve(w, h), id));
        }

        public static ICurve CreateCurve(double w, double h) {
            return CurveFactory.CreateRectangle(w, h, new Point()) ;
        }
    }
}