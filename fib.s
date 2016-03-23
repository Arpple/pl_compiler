.data
	prompt: .asciiz "Input the number: "
	newline: .asciiz "\n"
	current: .asciiz "curren number: "
	goToM1: .asciiz "Go to f(n-1) > "
	goToM2: .asciiz "Go to f(n-2) > "
	output: .asciiz "  Output : "
	return: .asciiz "     RETURN!!!!  \n"
	BUG: .asciiz "Why not jr !?"
.text

Main:
	li $v0,4
	la $a0,prompt
	syscall

	#read input
	li $v0,5
	syscall

	#go to function
	move $a0,$v0
	jal Fibonanciiz

	#when end of process
	li $v0,4
	la $a0,output
	syscall

	# prigt int
		move $a0,$s0
		li $v0,1
		syscall
		li $v0,10
		syscall
	#a0 has number for find fibonancii

	#fibo code is
	# int fibo(int n)
	# 	if n==0 return 1
	# 	if n==1 return 1
	# 	else return f(n-1)+f(n-2)

	Fibonanciiz:
		addi $sp,$sp,3 # use 2 parameter is [4=current n position][4=memory or last doing]
		save $ra,0($sp) #store pointer
		save $a0,1($sp) #store data (n)
		#save something,2($sp) #store data at f(n-1)

		load $a0,1($sp) #cause i use a0 for check print:\
		li $s0,1  #set defult return 1
		#if f(n) <= 1 return 1 (s0)
			blte $a0,$0,RETURNF0 #case f0
			li	$t7,1
			blte $a0,$t7,RETURNF1 #case f1
		#else
			load $a0,1($sp) #cause i use a0 for check print:\
			# F(N-1)
			addi $a0,$a0,-1 # and go to f(n-1)
			jal Fibonanciiz # jump to fibo n-1
			save $s0,2($sp) #store f(n-1) in stack cause when go f(n-2) data s1 has been change

			load $a0,1($sp) #cause i use a0 for check print:\
			#F(N-2)
			load $a0,1($sp) #load original data
			addi $a0,$a0,-2 # set f(n-2)
			jal Fibonanciiz # jump to fibo n-2
			move $s2,$s0
			load $s1,2($sp) #load f(n-1) values to s1 for calculatate

			#RETURN STATEMENT
			add $t0,$s1,$s2
			move $s0,$t0

	END_Fibonanciiz:
		load $ra,0($sp) #load before last position
		addi $sp,$sp,-3 #resotre pointer back -3 byte (3array)
		jr $ra

	RETURNF0:
		li $s0,0 #set f0
		jump END_Fibonanciiz
	RETURNF1:
		li $s0,1 #set f1
		jump END_Fibonanciiz
end:
