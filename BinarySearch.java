public class BinarySearch
{

	/*
	 * テーブルのエントリ
	*/
	static private class Entry
	{
		int key;        //比較の対象となるキー
		Object data;    //それ以外

		/**
		 *　エントリーの作成
		 *	@param key　キー
		 *　@param data キー　に対応するデータ
		 *
		 * */
		private Entry(int key, Object data)
		{
			this.key = key;
			this.data = data;
		}
	}
	final static int MAX = 100;
	Entry[] table = new Entry[MAX];
	int  n = 0;
	/**
	 * データを登録する
	 * @param key キー
	 * @param key キーに対応するデータ
	*/
	public void push(int key, Object data)
	{
		if (n >= MAX){
			throw new IllegalStateException("データの個数が多すぎます");
		}
		table[n++] = new Entry(key,data);
	}
	/**
	 *二分探索におけるデータの登録
	 **/
	public void add(int key,Object data)
	{
		int pos;
		pos = getAddPosion(key); //データを挿入するべき位置
		//配列中のposより後ろに要素を１つずつ後ろにずらす;
		for(int i = n - 1; i >= pos; i--){
			if(i + 1 < MAX){
				table[i + 1] = table[i];
			}
		}
		table[pos] = new Entry(key,data);

	}

	/**
	 *  キー keyに対応するデータを探す
	 *  @param key キー
	 *  @return キー key に対応するデータ。キーが見つからなければnullを返す
	 * */
	public Object search(int key)
	{
		int low  = 0;
		int high = n - 1;

		while(low <= high){
			int middle = (low + high) / 2;
			if(key == table[middle].key)
				return table[middle].data;
			else if(key < table[middle].key)
				high = middle - 1;
			else // key > table[middle].keyである
				low = middle + 1;
		}
		return null;
	}
	public static void main(String[] args){


		BinarySearch table = new BinarySearch();
		table.push(1,"一");
		table.push(3,"三");
		table.push(4,"四");
		table.push(8,"八");
		table.push(13,"一三");
		table.push(14,"一四");
		table.push(18,"一八");
		table.push(20,"二〇");
		table.push(21,"二一");
		table.push(25,"二五");
		table.add(10,"十");
		table.display();
	}
	public void display(){
		for(Entry data : table){
			if(data != null){
				System.out.println("key :" + data.key + "\t data : " +data.data);
			}
		}
	}
	public int getAddPosion(int key){

		int low  = 0;
		int high = n - 1;

		while(low <= high){
			int middle = (low + high) / 2;
			if(middle == 0){
				return middle;
			}else if(n -1 == middle){
				return middle;
			}
			if(table[middle].key >= key && table[middle - 1].key < key)
				return middle;
			else if (table[middle].key < key && table[middle + 1].key > key)
				return middle + 1;
			else if(key < table[middle].key)
				high = middle - 1;
			else // key > table[middle].keyである
				low = middle + 1;
		}
		return 0;
	}

}
