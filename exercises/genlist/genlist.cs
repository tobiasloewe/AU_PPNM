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
	}
	public void remove(int i){
		T[] newdata = new T[size - 1];	
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

public class genlistbetter<T>{
	public T[] data;
	public int size=0,capacity=8;
	public genlist(){ data = new T[capacity]; }
	public void add(T item){ /* add item to list */
		if(size==capacity){
			T[] newdata = new T[capacity*=2];
			System.Array.Copy(data,newdata,size);
			data=newdata;
			}
		data[size]=item;
		size++;
	}
}
