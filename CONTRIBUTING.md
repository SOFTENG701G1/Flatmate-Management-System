# Contribution Guidelines

These coding conventions are meant to serve as a directive to improve our program quality and our productivity. Prime consideration is given to the issues of program readability, understanding, ease of development and maintenance, ease of debugging, and program efficiency. 

Good programming practices and established conventions and standards should always be followed. Changes to improve machine efficiency should never obscure organized structured program logic.

These standards, conventions, and guidelines will provide for uniformity in the development, construction and installation of reliable program.

## Pull requests

Please ensure the following steps are carefully followed when making a PR.

### Only touch relevant files

Make sure your PR stays focused on a single feature. Don't change project configs or any files unrelated to the subject you're working. Open a single PR for each subject.

### Make sure your code is clean

Checkout the [project style guide](TO DO - insert coding convention guidelines), make sure your code is conformant and clean. Remove any debugging lines (`debuggers`, `console.log`).

### Make sure you unit test your changes

Adding a feature? Make sure you add unit tests to support it.

Fixing a bug? Make sure you added a test reproducing the issue.

### Make sure tests pass

All our projects' unit tests can be run by typing `yarn test` at the root of the project. You may need to install dependencies like `mocha`, `chai`, or `sinon`.

### Keep your commit history short and clean

In a large project, it is important to keep the git history clean and tidy. This helps to identify the causes of bugs and helps in identifying the best fixes.

Keeping the history clean means making one commit per feature. It also means squashing every fix you make on your branch after team review.

### Be descriptive

Write a convincing description of your PR and why we should land it.

### Hang on during code review

It is important for us to keep the core code clean and consistent. This means we're pretty hard on code review!

Code reviews are the best way to improve ourselves as engineers. Don't take the reviews personally: they're there to help us improve.

Above steps should be used when following the steps belowwhen you'd like your work considered for inclusion in the
project:

1. [Fork](http://help.github.com/fork-a-repo/) the project, clone your fork,
   and configure the remotes:

   ```bash
   # Clone your fork of the repo into the current directory
   git clone https://github.com/<your-username>/<repo-name>
   # Navigate to the newly cloned directory
   cd <repo-name>
   # Assign the original repo to a remote called "upstream"
   git remote add upstream https://github.com/<upstream-owner>/<repo-name>
   ```

2. If you cloned a while ago, get the latest changes from upstream:

   ```bash
   git checkout <dev-branch>
   git pull upstream <dev-branch>
   ```

3. Create a new topic branch (off the main project development branch) to
   contain your feature, change, or fix:

   ```bash
   git checkout -b <topic-branch-name>
   ```

4. Commit your changes in logical chunks. Please adhere to these [git commit
   message guidelines](http://tbaggery.com/2008/04/19/a-note-about-git-commit-messages.html)
   or your code is unlikely be merged into the main project. Use Git's
   [interactive rebase](https://help.github.com/articles/interactive-rebase)
   feature to tidy up your commits before making them public.

5. Locally merge (or rebase) the upstream development branch into your topic branch:

   ```bash
   git pull [--rebase] upstream <dev-branch>
   ```

6. Push your topic branch up to your fork:

   ```bash
   git push origin <topic-branch-name>
   ```

7. [Open a Pull Request](https://help.github.com/articles/using-pull-requests/)
    with a clear title and description.

<a name="bugs"></a>
## Bug reports

A bug is a _demonstrable problem_ that is caused by the code in the repository.
Good bug reports are extremely helpful - thank you!

Guidelines for bug reports:

1. **Use the GitHub issue search** &mdash; check if the issue has already been
   reported.

2. **Check if the issue has been fixed** &mdash; try to reproduce it using the
   latest `master` or development branch in the repository.

3. **Isolate the problem** &mdash; create a [reduced test
   case](http://css-tricks.com/reduced-test-cases/) and a live example.

A good bug report shouldn't leave others needing to chase you up for more
information. Please try to be as detailed as possible in your report. What is
your environment? What steps will reproduce the issue? What browser(s) and OS
experience the problem? What would you expect to be the outcome? All these
details will help people to fix any potential bugs.

Example:

> Short and descriptive example bug report title
>
> A summary of the issue and the browser/OS environment in which it occurs. If
> suitable, include the steps required to reproduce the bug.
>
> 1. This is the first step
> 2. This is the second step
> 3. Further steps, etc.
>
> `<url>` - a link to the reduced test case
>
> Any other information you want to share that is relevant to the issue being
> reported. This might include the lines of code that you have identified as
> causing the bug, and potential solutions (and your opinions on their
> merits).

<a name="guidelines"></a>
## Coding Guidelines 

- [React JS Coding Conventions](reactjs-guidelines.md)

- [C# Coding Conventions](C#-guidelines.md)
 
Additional Notes
  Please ensure you have also ready [Code of Conduct](CODE_OF_CONDUCT.md)
