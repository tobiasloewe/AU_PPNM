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
    public static matrix inverse2(matrix A){
        int n = A.size1; 
        matrix A_inv = new matrix(n, n);
        for (int i = 0; i < n; i++) {
            vector e_i = new vector(n);
            e_i[i] = 1;
            (matrix Q, matrix R) = decomp(A);
            vector x = solve(Q, R, e_i);
            for (int j = 0; j < n; j++) {
                A_inv[j, i] = x[j];
            }
        }
        return A_inv;
    }
    public static matrix inverse(matrix A) {
    int m = A.size1, n = A.size2;
    
    if (m == n) {
        // Square matrix: Use standard QR-based inverse
        matrix A_inv = new matrix(n, n);
        for (int i = 0; i < n; i++) {
            vector e_i = new vector(n);
            e_i[i] = 1;
            (matrix q, matrix r) = decomp(A);
            vector x = solve(q, r, e_i);
            for (int j = 0; j < n; j++) {
                A_inv[j, i] = x[j];
            }
        }
        return A_inv;
    } else {
        // Non-square case: Compute pseudo-inverse A^+
        (matrix Q, matrix R) = decomp(A);
        if (m > n) {
            // Tall matrix (m > n): A⁺ = (Aᵀ A)⁻¹ Aᵀ
            matrix At = A.T;
            matrix AtA = At * A;
            matrix AtA_inv = inverse(AtA); // Recursive call on square matrix
            return AtA_inv * At;
        } else {
            // Wide matrix (m < n): A⁺ = Aᵀ (A Aᵀ)⁻¹
            matrix At = A.T;
            matrix AAt = A * At;
            matrix AAt_inv = inverse(AAt); // Recursive call on square matrix
            return At * AAt_inv;
        }
    }
    }

    public static (matrix, matrix) decomp(matrix Aext){
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
        return (Qext, Rext);
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