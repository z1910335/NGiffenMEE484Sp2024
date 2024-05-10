//============================================================================
// SpinBook.cs  
// Class for defining the dynamics of a spinning "book".
//============================================================================
using System;
using SimUtils;
using Godot;

namespace Sim
{    
    public class SpinBook : Simulator
    {
        private double I1 = 1.0;   // Principal moments of inertia
        private double I2 = 2.0;
        private double I3 = 3.0;

        //--------------------------------------------------------------------
        // constructor
        //--------------------------------------------------------------------
        public SpinBook() : base(7)
        {
            
            // default initial conditions
            x[0] = 1.0;    // q0
            x[1] = 0.0;    // q1
            x[2] = 0.0;    // q2
            x[3] = 0.0;    // q3
            x[4] = 0.001;    // u1   body angular velocity
            x[5] = 3.0;   // u2   body angular velocity
            x[6] = 0.0;    // u3   body angular velocity
            //x[7] = x[5] / I1;
            //x[8] = x[6] / I2;
            //x[9] = x[7] / I3;
            SetRHSFunc(RHSFunc);
        }

        //--------------------------------------------------------------------
        // rhsFunc: function which calculates the right side of the
        //          differential equation.
        //--------------------------------------------------------------------
        public void RHSFunc(double[] st, double t, double[] ff)
        {
            double Q0 = st[0];
            double Q1 = st[1];
            double Q2 = st[2];
            double Q3 = st[3];

            double u1 = st[4];
            double u2 = st[5];
            double u3 = st[6];

            // kinematic differential equations
            ff[0] = 0.5*(-Q1*u1 - Q2*u2 - Q3*u3);
            ff[1] = 0.5*( Q0*u1 - Q3*u2 + Q2*u3);
            ff[2] = 0.5*( Q3*u1 + Q0*u2 - Q1*u3);
            ff[3] = 0.5*(-Q2*u1 + Q1*u2 + Q0*u3);
    
            // kinetics
            ff[4] = (I2 - I3)*u2*u3/I1;
            ff[5] = (I3 - I1)*u1*u3/I2;
            ff[6] = (I1 - I2)*u1*u2/I3;

            // Omega/MoI
            //ff[7] = omega1/I1;
            //ff[8] = omega2/I2;
            //ff[9] = omega3/I3;

        }

        //--------------------------------------------------------------------
        // getters/setters
        //--------------------------------------------------------------------
        public double q0
        {
            get{ return x[0]; }
        }

        public double q1
        {
            get{ return x[1]; }
        }
        public double q2
        {
            get{ return x[2]; }
        }
        public double q3
        {
            get{ return x[3]; }
        }
        public double omega1
        {
            get { return x[4]; }
            set 
            { 
                x[4] = value; 
                GD.Print("Setting omega1 in SpinBook: ", x[4]);
            }
        }

        public double omega2
        {
            get { return x[5]; }
            set 
            { 
                x[5] = value; 
                GD.Print("Setting omega2 in SpinBook: ", x[5]);
            }
        }

        public double omega3
        {
            get { return x[6]; }
            set 
            { 
                x[6] = value; 
                GD.Print("Setting omega3 in SpinBook: ", x[6]);
            }
        }
        /*
        public double omega1Norm
        {
            get{ return x[7]; }

            set{ x[7] = value; }
        }
        public double omega2Norm
        {
            get{ return x[8]; }

            set{ x[8] = value; }
        }
        public double omega3Norm
        {
            get{ return x[9]; }

            set{ x[9] = value; }
        }
        */
        public double IG1
        {
            get{ return I1; }

            set
            {
                if(value >= 0.1)
                    I1 = value;
            }
        }
        public double IG2
        {
            get{ return I2; }

            set
            {
                if(value >= 0.1)
                    I2 = value;
            }
        }
        public double IG3
        {
            get{ return I3; }

            set
            {
                if(value >= 0.1)
                    I3 = value;
            }
        }
    }
}
