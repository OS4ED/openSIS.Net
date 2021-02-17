export interface SearchStudentCourseSection {
    courseSelected?: boolean;
    course: string;
    courseSection: string;
    markingPeriod: string;
    startDate: string;
    endDate: string
    seats?: number;
    available?: number;
    scheduledTeacher?: boolean;
}