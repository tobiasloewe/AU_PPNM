using System;
using static System.Math;

public class QR{
    public matrix Q { get; private set; }
    public matrix R { get; private set; }
    public vector b { get; private set; }


    public QR(matrix A, vector c){
        int m = A.size2;
        Q=A.copy();
        b=c.copy();
        R=new matrix(m,m);
        for(int i=0;i<m; i++){
            R[i,i]=Q[i].norm(); /* Q[ i ] points to the i−th columb */ 
            Q[i]/=R[i,i]; 
            for(int j=i+1;j<m; j++){
                R[i,j]=Q[i].dot(Q[j]);
                Q[j]-=Q[i]*R[i,j];
            }
        }
    }
    public void solve(){
        System.Console.WriteLine($"{b.size} vs {Q.size1} x{Q.size2}");
        b = Q.T*b;
        for(int i=this.b.size -1; i>=0; i--){
            double sum=0;
            for(int k=i+1; k<this.b.size; k++) sum+=R[i,k]*this.b[k];
            b[i]=(b[i]-sum)/R[i,i];
        }
    }
    public vector getSolve(){
        vector Qtb = Q.T * b;

        int n = R.size1;
        vector x = new vector(n);

        for (int i = n - 1; i >= 0; i--){
            double sum = 0;
            for (int j = i + 1; j < n; j++){
                sum += R[i, j] * x[j];
            }
            x[i] = (Qtb[i] - sum) / R[i, i];
        }

        return x;
    }





}