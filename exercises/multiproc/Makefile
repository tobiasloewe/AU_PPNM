CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
#TIME = time --portability --append --output $@
TIME = time --format "$$nthreads %e %U" --append --output $@

MAX_THREADS = 64
N=5e7

nthreadlist = $(shell seq 1 $(MAX_THREADS))

all: plot.threads.svg

Out.txt : main.exe Makefile
	>$@
	
	@for nthreads in $(nthreadlist); do $(TIME) mono $< -terms $(N) -threads $$nthreads >>$@; done
	
	#printf "\n this was manual threading\n" >>$@
	$(TIME) mono $< -terms $N -threads 1 -mode 1 2>> log1.txt >>$@
	@echo " " >>$@
	@echo "this was mode 1\n" >>$@
	
	$(TIME) mono $< -terms $N -threads 1 -mode 2 2>> log2.txt >>$@
	@echo "this was mode 2\n" >>$@
	@echo " " >>$@
	@echo "Computation finished.\n Mode 2 might have more scheduling overhead than our simple multithreading approach and is therefore slower" >>$@
main.exe: main.cs; $(MKEXE)

plot.threads.svg: Out.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key right top; \
		set xlabel "n threads"; \
		set ylabel "runtime [s]"; \
		set grid; \
		plot \
		    "Out.txt" every ::0::$(shell expr $(MAX_THREADS) - 1) using 1:2 with linespoints title "Total elapsed time", \
		    "Out.txt" every ::0::$(shell expr $(MAX_THREADS) - 1) using 1:3 with linespoints title "User CPU time"; \
	' | tee log.lngamma.gpi | gnuplot



clean:
	rm -f *.exe *.dll Out.txt *.svg
