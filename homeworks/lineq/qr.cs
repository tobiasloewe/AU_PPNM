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
        this.b = Q.T*this.b;
        for(int i=this.b.size -1; i>=0; i--){
            double sum=0;
            for(int k=i+1; k<this.b.size; k++) sum+=this.R[i,k]*this.b[k];
            this.b[i]=(this.b[i]-sum)/this.R[i,i];
        }
    }

    public static matrix inverse(matrix A){
        int n = A.size1; 
        matrix A_inv = new matrix(n, n);
        for (int i = 0; i < n; i++) {
            vector e_i = new vector(n);
            e_i[i] = 1;
            var temp = decomp(A);
            vector x = solve(temp[0], temp[1], e_i);
            for (int j = 0; j < n; j++) {
                A_inv[j, i] = x[j];
            }
        }
        return A_inv;
    }

    public static matrix[] decomp(matrix Aext){
        int m = Aext.size2;
        matrix Qext = Aext.copy();
        matrix Rext =new matrix(m,m);
        for(int i=0;i<m; i++){
            Rext[i,i]=Qext[i].norm(); /* Q[ i ] points to the i−th columb */ 
            Qext[i]/=Rext[i,i]; 
            for(int j=i+1;j<m; j++){
                Rext[i,j]=Qext[i].dot(Qext[j]);
                Qext[j]-=Qext[i]*Rext[i,j];
            }
        }
        matrix[] results = new matrix[2] {Rext, Qext}; 
        return results;
    }

    public static vector solve(matrix Rext, matrix Qext, vector bext){
        bext = Qext.T*bext;
        for(int i=bext.size -1; i>=0; i--){
            double sum=0;
            for(int k=i+1; k<bext.size; k++) sum+=Rext[i,k]*bext[k];
            bext[i]=(bext[i]-sum)/Rext[i,i];
        }
        return bext;
    }




}