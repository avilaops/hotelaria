# Security Policy

## Supported Versions

Use this section to tell people about which versions of your project are currently being supported with security updates.

| Version | Supported          |
| ------- | ------------------ |
| 2.6.x   | :white_check_mark: |
| 2.5.x   | :white_check_mark: |
| 2.4.x   | :x:                |
| < 4.0   | :x:                |

## Reporting a Vulnerability

Use this section to tell people how to report a vulnerability.

Tell them where to go, how often they can expect to get an update on a reported vulnerability, what to expect if the vulnerability is accepted or declined, etc.

**Where to Report:**
- Email: dev@avila.inc
- Subject: "[SECURITY] Description of the issue"

**What to Include:**
- Description of the vulnerability
- Steps to reproduce
- Potential impact
- Suggested fix (if any)

**Response Time:**
- Initial response: Within 48 hours
- Status updates: Every 7 days
- Fix timeline: Depends on severity
  - Critical: 24-48 hours
  - High: 3-7 days
  - Medium: 1-2 weeks
  - Low: Next release cycle

**After Reporting:**
- **Accepted**: We'll work on a fix and credit you in the security advisory
- **Declined**: We'll explain why it doesn't qualify as a security issue

## Security Measures

This project implements:
- ✅ Scoped authentication (session isolation)
- ✅ Rate limiting on login attempts
- ✅ PBKDF2 password hashing (100k iterations)
- ✅ Input sanitization and validation
- ✅ HTTPS enforcement
- ✅ Security headers (CSP, X-Frame-Options, etc.)
- ✅ CSRF protection
- ✅ Audit logging (LGPD compliant)

## Known Vulnerabilities

Currently, there are no known vulnerabilities in the supported versions.

For historical security advisories, see: https://github.com/avilaops/hotelaria/security/advisories

## Security Best Practices for Users

1. **Strong Passwords**: Use passwords with at least 12 characters
2. **Update Regularly**: Keep your installation up to date
3. **HTTPS Only**: Never deploy without HTTPS
4. **Review Logs**: Monitor audit logs regularly
5. **Backup Data**: Regular backups prevent data loss

## Contact

For security-related questions that are not vulnerabilities:
- Email: dev@avila.inc
- GitHub Discussions: https://github.com/avilaops/hotelaria/discussions

---

**Copyright © 2026 Ávila Inc. - All rights reserved**
