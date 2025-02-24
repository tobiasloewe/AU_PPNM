using System;
using static System.Math;

public class QR{
    public readonly matrix Q;
    public readonly matrix R;
    public QR(matrix A){
        int m = A.size2;
        Q=A.copy();
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



}