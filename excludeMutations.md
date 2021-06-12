

# Exclude Mutations
To exclude mutations one can add a list of group Id's to the command line with the appropraite tag when running Faultify. You can also exclude singular mutations by either using a list  on the command line with the appropriate tag or a filepath to a JSON file.

# Mutations
Document that shows Faultify's mutations by group. The id's can be used to exclude them from being used during the mutation process.

## Faultify.Analyze/Analyzers/ArithmeticAnalyzer.cs

### Group Id: Arithmetic

| symbol    |Mutation	| ID		|Level                      |
|-----------|-----------|-----------|----------|
|+		|-            |addToSub      | Simple
|+		|*            |addToMul      | Medium
|+		|/            |addToDiv      | Detailed
|+		|%            |addToRem      | Detailed
|-		|+            |subToAdd      | Simple
|-		|*            |subToMul      | Medium
|-		|/            |subToDiv      | Detailed
|-		|%            |subToRem      | Detailed
|*		|+            |mulToAdd      | Simple
|*		|-            |mulToSub      | Medium
|*		|/            |mulToDiv      | Detailed
|*		|%            |mulToRem      | Detailed
|/		|+            |divToAdd      | Simple
|/		|*            |divToMul      | Medium
|/		|-            |divToSub      | Detailed
|/		|%            |divToRem      | Detailed
|%		|+            |remToAdd      | Simple
|%		|*            |remToMul      | Medium
|%		|/            |remToDiv      | Detailed
|%		|-            |remToSub      | Detailed


##  Faultify.Analyze/Analyzers/ArrayAnalyzer.cs
### Group Id: Array

## Faultify.Analyze/Analyzers/BitwiseAnalyzer.cs
### Group Id: Bitwise
| symbol    |Mutation	| ID		|Level                         |
|-----------|-----------|-----------|----------|
|&#124; |&            |orToAnd      | Simple
|&#124;	|^            |orToXor      | Medium
|&		|&#124;       |andToOr      | Simple
|&		|^            |andToXor     | Medium
|^ 		|&#124;       |xorToOr	    | Simple
|^		|&            |xorToAnd     | Medium

## Faultify.Analyze/Analyzers/BranchingAnalyzer.cs
### Group Id: Branching
| symbol    |Mutation	| ID		|Level                         |
|-----------|-----------|-----------|----------|
|true 			|false           |trueToFalse    | Simple
|true (short)	|false (short)   |trueToFalseS   | Simple
|false 			|true            |falseToTrue    | Simple
|false (short)	|true (short)    |falseToTrueS   | Simple




## Faultify.Analyze/Analyzers/ComparisonAnalyzer.cs
### Group Id: Comparison

| symbol    |Mutation	|Branching	| ID		|Level                       |
|-----------|-----------|-----------|-----------|----------|
|==				|!=				|yes	|eqToNeq		|Simple
|>=				|<				|yes	|greToLt		|Simple
|>=	(unordered)	|< (unordered)	|yes	|greToLtUn		|Simple
|>				|<=				|yes	|grToLte		|Simple
|>	(unordered)	|<= (unordered)	|yes	|grToLteUn		|Simple
|<=		|>				|yes	|LteToGr		|Simple
|<=	(unordered)	|> (unordered)	|yes	|LteToGrUn		|Simple
|<				|>=				|yes	|LtToGre		|Simple
|<	(unordered)	|>= (unordered)	|yes	|LtToGreUn		|Simple
|!=				|==				|yes	|neqToEq		|Simple
|==				|<				|no		|eqToLtNB		|Simple
|<				|>				|no		|ltToGrNB		|Simple
|<	(unordered)	|>	(unordered)	|no		|ltToGrUnNB		|Simple
|>				|<				|no		|grToLtNB		|Simple
|>	(unordered)	|<	(unordered)	|no		|grToLtUnNB		|Simple

## Faultify.Analyze/Analyzers/ConstantAnalyzer.cs
### Group Id: Constant

## Faultify.Analyze/Analyzers/LogicalAnalyzer.cs
### Group Id: Logical
see BitwiseAnalyzer
|Symbol| Bitwise Symbol|
|----|-----|
|&&| &
|&#124;&#124;|&#124;

## Faultify.Analyze/Analyzers/VariableAnalyzer.cs
### Group Id: Variable
