# Using Pattern Matching to Avoid Massive "if" Statements

## [Medium Article]()

With the increasing complexity of business logic in our applications, the "if" statements in the code will become more and more bloated, thus hard to read and maintain. One word of caution: I don't mean to say that "if" statements are evil or code-smell. I do believe "if" statements have their advantages in some use cases, but for the use cases discussed today, we will try to find an alternative way to write easy-to-read code.

This blog post  will show you a way to avoid clunky or nested "if" statements by using Pattern Matching and decision tables. I will first explain the use cases of decision tables, then illustrate the implementation using pattern matching in C#.