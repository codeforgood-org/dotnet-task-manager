# Security Policy

## Supported Versions

We release patches for security vulnerabilities for the following versions:

| Version | Supported          |
| ------- | ------------------ |
| 2.x.x   | :white_check_mark: |
| 1.x.x   | :x:                |

## Reporting a Vulnerability

The Task Manager CLI team takes security bugs seriously. We appreciate your efforts to responsibly disclose your findings.

### How to Report a Security Vulnerability

**Please do not report security vulnerabilities through public GitHub issues.**

Instead, please report them via one of the following methods:

1. **GitHub Security Advisories** (Preferred)
   - Go to the [Security tab](https://github.com/codeforgood-org/dotnet-task-manager/security)
   - Click on "Report a vulnerability"
   - Provide detailed information about the vulnerability

2. **Email**
   - Send an email to security@codeforgood.org
   - Include the word "SECURITY" in the subject line
   - Encrypt your message using our PGP key (if available)

### What to Include in Your Report

Please include the following information in your report:

- Type of issue (e.g., buffer overflow, SQL injection, cross-site scripting)
- Full paths of source file(s) related to the manifestation of the issue
- The location of the affected source code (tag/branch/commit or direct URL)
- Any special configuration required to reproduce the issue
- Step-by-step instructions to reproduce the issue
- Proof-of-concept or exploit code (if possible)
- Impact of the issue, including how an attacker might exploit it

### Response Timeline

- **Initial Response**: Within 48 hours of receiving your report
- **Status Update**: Within 7 days with an assessment and tentative fix timeline
- **Resolution**: We aim to resolve critical security issues within 30 days

### Disclosure Policy

- Security issues are kept confidential until a fix is released
- After a fix is released, we will:
  - Publish a security advisory
  - Credit the reporter (unless anonymity is requested)
  - Update the CHANGELOG with security fix information

### Safe Harbor

We support safe harbor for security researchers who:

- Make a good faith effort to avoid privacy violations, data destruction, and service interruption
- Only interact with accounts you own or with explicit permission of the account holder
- Do not exploit a security issue beyond the minimum necessary to demonstrate it
- Report vulnerabilities promptly
- Keep vulnerability details confidential until a fix is released

We will not pursue legal action against researchers who follow these guidelines.

## Security Best Practices for Users

### Data Protection

1. **File Permissions**
   - Ensure `tasks.json` has appropriate file permissions (not world-readable)
   - On Unix systems: `chmod 600 tasks.json`

2. **Backup Your Data**
   - Regularly backup your `tasks.json` file
   - Use `taskman export` to create encrypted backups if needed

3. **Sensitive Information**
   - Do not store passwords, API keys, or sensitive data in task descriptions
   - Use tags instead of including sensitive details in descriptions

### Docker Security

1. **Container Isolation**
   - Run containers with least privilege
   - Use read-only volumes where possible
   - Don't run as root inside containers

2. **Image Security**
   - Pull images from official sources only
   - Regularly update to latest image versions
   - Scan images for vulnerabilities

### Dependencies

We regularly update dependencies to address security vulnerabilities:

- Monitor GitHub security advisories
- Run `dotnet list package --vulnerable` to check for vulnerable packages
- Update dependencies promptly when security fixes are available

### Building from Source

When building from source:

1. Verify the source code integrity
2. Review the code for any suspicious changes
3. Build in a clean environment
4. Verify checksums of dependencies

## Security Features

### Current Security Measures

1. **Input Validation**
   - All user inputs are validated
   - Arguments are sanitized before processing
   - No command injection vulnerabilities

2. **File Operations**
   - Controlled file access (fixed filename)
   - No arbitrary file read/write operations
   - Safe JSON deserialization with type checking

3. **Dependency Security**
   - Minimal external dependencies
   - Official Microsoft packages only (except Spectre.Console)
   - Regular security audits

4. **Code Quality**
   - Static code analysis
   - Automated testing
   - Code review process for all changes

### Planned Security Enhancements

- [ ] Add encryption support for sensitive task data
- [ ] Implement audit logging for all operations
- [ ] Add data integrity checks (checksums)
- [ ] Support for signing/verification of task files
- [ ] Multi-user access controls
- [ ] Rate limiting for operations

## Security Audit History

| Date       | Type                | Findings | Status   |
|------------|---------------------|----------|----------|
| 2024-01-15 | Initial assessment  | None     | Complete |

## Vulnerability Disclosure Timeline

No vulnerabilities have been disclosed at this time.

## Contact

For security-related questions or concerns:

- Security Team: security@codeforgood.org
- Project Maintainer: Via GitHub issues (for non-security questions)

## Acknowledgments

We would like to thank the following researchers for responsibly disclosing security issues:

<!-- Names will be added as security issues are reported and resolved -->

---

**Note**: This security policy may be updated at any time. Please check back regularly for updates.

Last Updated: 2024-01-15
