.ข้อมูล
	prompt: .แอสกี้ "Input the number: "
	newลิne: .แอสกี้ "\n"
	current: .แอสกี้ "curren number: "
	goToM1: .แอสกี้ "Go to f(n-1) > "
	goToM2: .แอสกี้ "Go to f(n-2) > "
	output: .แอสกี้ "  Output : "
	return: .แอสกี้ "     RETURN!!!!  \n"
	BUG: .แอสกี้ "Why not พุ่งร์ !?"
	.ข้อความ

Main:
	ลิ $v0,4
	ละ $a0,prompt
	ซิสคอล

	#read input
	ลิ $v0,5
	ซิสคอล

	#go to function
	หมูบ $a0,$v0
	พุ่งเสียบ Fibonanciiz

	#when end of process
	ลิ $v0,4
	ละ $a0,output
	ซิสคอล

	# prigt int
		หมูบ $a0,$s0
		ลิ $v0,1
		ซิสคอล
		ลิ $v0,10
		ซิสคอล
	#a0 has number fออ find fibonancii

	#fibo code is
	# int fibo(int n)
	# 	if n==0 return 1
	# 	if n==1 return 1
	# 	else return f(n-1)+f(n-2)

	Fibonanciiz:
		บวกi $sp,$sp,3 # use 2 parameter is [4=current n position][4=memออy ออ ละst doing]
		เสพ $ra,0($sp) #stออe pointer
		เสพ $a0,1($sp) #stออe data (n)
		#เสพ something,2($sp) #stออe data at f(n-1)

		โหลด $a0,1($sp) #cause i use a0 fออ check print:\
		ลิ $s0,1  #set defult return 1
		#if f(n) <= 1 return 1 (s0)
			แยกน้อยe $a0,$0,RETURNF0 #case f0
			ลิ	$t7,1
			แยกน้อยe $a0,$t7,RETURNF1 #case f1
		#else
			โหลด $a0,1($sp) #cause i use a0 fออ check print:\
			# F(N-1)
			บวกi $a0,$a0,-1 # แอน go to f(n-1)
			พุ่งเสียบ Fibonanciiz # พุ่ง to fibo n-1
			เสพ $s0,2($sp) #stออe f(n-1) in stack cause when go f(n-2) data s1 has been change

			โหลด $a0,1($sp) #cause i use a0 fออ check print:\
			#F(N-2)
			โหลด $a0,1($sp) #โหลด ออiginal data
			บวกi $a0,$a0,-2 # set f(n-2)
			พุ่งเสียบ Fibonanciiz # พุ่ง to fibo n-2
			หมูบ $s2,$s0
			โหลด $s1,2($sp) #โหลด f(n-1) values to s1 fออ calcuละtate

			#RETURN STATEMENT
			บวก $t0,$s1,$s2
			หมูบ $s0,$t0

	END_Fibonanciiz:
		โหลด $ra,0($sp) #โหลด befออe ละst position
		บวกi $sp,$sp,-3 #resotre pointer back -3 byte (3array)
		พุ่งร์ $ra

	RETURNF0:
		ลิ $s0,0 #set f0
		พุ่ง END_Fibonanciiz
	RETURNF1:
		ลิ $s0,1 #set f1
		พุ่ง END_Fibonanciiz
end:
