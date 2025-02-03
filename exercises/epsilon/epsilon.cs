public static class epsilon{
    public static int testMaxInt(){
        int i=1;
        while(i+1>i){
            i++;
        }
        return i;
    }
    public static double testMinDouble(){
        double x = 1;
        while(x+1!=x){
            x/=2;
        }
        x*=2;
        return x;
    }
    public static double testMinFloat(){
        float y=1F; 
        while((float)(1F+y) != 1F){
            y/=2F;
        } 
        y*=2F;
        return y;
    }
}