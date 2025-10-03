-- Migration: Add showcase type support
-- Place this in: database/init/02-showcase-types-migration.sql

-- Add new columns to showcases table
ALTER TABLE showcases 
ADD COLUMN showcase_type VARCHAR(50) NOT NULL DEFAULT 'image_rendering' 
    CHECK (showcase_type IN ('image_rendering', 'text_generation')),
ADD COLUMN prompt TEXT NOT NULL DEFAULT '',

-- Image Rendering fields (both required for this type)
ADD COLUMN source_image_url VARCHAR(500),
ADD COLUMN result_image_url VARCHAR(500),

-- Text Generation fields
ADD COLUMN generated_text TEXT,
ADD COLUMN source_text TEXT,
ADD COLUMN source_file_url VARCHAR(500),
ADD COLUMN source_file_type VARCHAR(10) 
    CHECK (source_file_type IS NULL OR source_file_type IN ('pdf', 'docx', 'text'));

-- Remove defaults after adding columns (they were just for the migration)
ALTER TABLE showcases 
ALTER COLUMN showcase_type DROP DEFAULT,
ALTER COLUMN prompt DROP DEFAULT;

-- Add check constraints to ensure data integrity
ALTER TABLE showcases
ADD CONSTRAINT check_image_rendering_fields 
    CHECK (
        (showcase_type != 'image_rendering') OR 
        (source_image_url IS NOT NULL AND result_image_url IS NOT NULL)
    );

ALTER TABLE showcases
ADD CONSTRAINT check_text_generation_fields 
    CHECK (
        (showcase_type != 'text_generation') OR 
        (generated_text IS NOT NULL AND 
         (source_text IS NOT NULL OR source_file_url IS NOT NULL))
    );

ALTER TABLE showcases
ADD CONSTRAINT check_source_consistency
    CHECK (
        (showcase_type != 'text_generation') OR
        (
            (source_text IS NOT NULL AND source_file_url IS NULL) OR
            (source_text IS NULL AND source_file_url IS NOT NULL)
        )
    );

-- Add indexes for filtering by type
CREATE INDEX idx_showcases_type ON showcases(showcase_type, created_at DESC);

-- Add comment for documentation
COMMENT ON COLUMN showcases.showcase_type IS 'Type of showcase: image_rendering or text_generation';
COMMENT ON COLUMN showcases.prompt IS 'AI prompt used to generate the content';
COMMENT ON COLUMN showcases.source_image_url IS 'MinIO URL for source image (image_rendering only)';
COMMENT ON COLUMN showcases.result_image_url IS 'MinIO URL for result image (image_rendering only)';
COMMENT ON COLUMN showcases.generated_text IS 'AI-generated text content (text_generation only)';
COMMENT ON COLUMN showcases.source_text IS 'Plain text source/context (text_generation only, mutually exclusive with source_file_url)';
COMMENT ON COLUMN showcases.source_file_url IS 'MinIO URL for PDF/DOCX source file (text_generation only, mutually exclusive with source_text)';
COMMENT ON COLUMN showcases.source_file_type IS 'Type of source file: pdf, docx, or text';