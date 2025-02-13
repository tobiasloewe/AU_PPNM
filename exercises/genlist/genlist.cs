public class genlist<T>{
	public T[] data;
	public int size => data.Length; // property
	public T this[int i] => data[i]; // indexer
	public genlist(){ data = new T[0]; }
	public void add(T item){ 
		T[] newdata = new T[size+1];
		System.Array.Copy(data,newdata,size);
		newdata[size]=item;
		data=newdata;
	public void remove(int i){
		T[] newdata = new T[size - 1]
		System.Array.Copy(data,newdata,size);
		for (int j = 0; j<size-1;j++){
			if (j<i){
				newdata[j] = data[j];
			}
			else {
				newdata[j] = data[j+1];
			}
		}
		data = newdata;
	}
	}
}