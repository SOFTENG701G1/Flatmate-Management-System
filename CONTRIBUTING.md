# Contribution Guidelines

These coding conventions are meant to serve as a directive to improve our program quality and our productivity. Prime consideration is given to the issues of program readability, understanding, ease of development and maintenance, ease of debugging, and program efficiency. 

Good programming practices and established conventions and standards should always be followed. Changes to improve machine efficiency should never obscure organized structured program logic.

These standards, conventions, and guidelines will provide for uniformity in the development, construction and installation of reliable program.

For any document changes, please submit a change request using: [Document Change](https://github.com/SOFTENG701G1/Flatmate-Management-System/issues/new?assignees=&labels=documentation&template=documentation-update.md&title=%5BDOC%5D)

For any feature requests, please submit a feature request using: [Feature Request](https://github.com/SOFTENG701G1/Flatmate-Management-System/issues/new?assignees=&labels=feature&template=feature_request.md&title=%5BFEATURE%5D)

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

Front-end tests can be ran with `npm test` (there are none currently).
Back-end tests can be ran in Visual Studio (or your preferred IDE).

### Keep your commit history short and clean

In a large project, it is important to keep the git history clean and tidy. This helps to identify the causes of bugs and helps in identifying the best fixes.

Keeping the history clean means making one commit per feature. It also means squashing every fix you make on your branch after team review.

### Be descriptive

Write a convincing description of your PR and why we should land it.

### Code review

It is important for us to keep the core code clean and consistent. Please ensure any pull request have been reviewed by at least one of the team members.

### Steps for pull request

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

3. Create a new topic branch (off the main project development branch) with one of the following commands:

   ```bash
   git checkout -b bug/#id-some-bug
   git checkout -b feature/#id-some-feature
   git checkout -b documentation/#id-some-documents
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

6. Push your topic branch up to your fork with one of the following commands:

   ```bash
   git push origin bug/#id-some-bug
   git push origin feature/#id-some-feature
   git push origin documentation/#id-some-documents
   ```

7. [Open a Pull Request](https://help.github.com/articles/using-pull-requests/)
    with a clear title and description. Make sure at least one of our team members have reviewed the change. A review can be requested by following these steps: [Request Pull Request Review](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/requesting-a-pull-request-review)

<a name="bugs"></a>
## Bug reports

Guidelines for bug reports:

1. **Use the GitHub issue search** &mdash; check if the issue has already been
   reported.

2. **Check if the issue has been fixed** &mdash; try to reproduce it using the
   latest `master` or development branch in the repository.

3. **Create bug report** &mdash; Please fill out [Bug Report Template](https://github.com/SOFTENG701G1/Flatmate-Management-System/issues/new?assignees=&labels=bug&template=bug_report.md&title=%5BBUG%5D).

<a name="guidelines"></a>
## Coding Guidelines 

Depending on if you're working on the frontend or backend, please see below for language specific coding guidelines. 

- [React JS Coding Conventions](docs/reactjs-guidelines.md)

- [C# Coding Conventions](docs/C#-guidelines.md)
 
<a name="notes"></a>
## Additional Note 

- Please ensure you have also read [Code of Conduct](CODE_OF_CONDUCT.md)

<a name="sources"></a>
## Source

- This contribution guidline is based on the following sources

- [Pull Request Guidelines](https://github.com/exercism/docs/blob/master/contributing/pull-request-guidelines.md)

- [Strict Flow and GitHub Project Guidelines](https://gist.github.com/rsp/057481db4dbd999bb7077f211f53f212)

- [Git Merge Branching Good Practices](https://github.com/geonetwork/core-geonetwork/wiki/Git-merge-branching-good-practices)
